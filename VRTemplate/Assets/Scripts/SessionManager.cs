using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Transporting;
using System;
using System.Threading;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField]
    private NetworkManager networkManager;

    [SerializeField]
    private SceneManager sceneManager;

    [SerializeField]
    private SceneReference lobbyScene;

    [SerializeField]
    private SceneReference redRoomScene;

    [SerializeField]
    private string address;

    [SerializeField]
    private ushort port;

    [SerializeField]
    private float serverTimeToLiveInSeconds;

    /// <summary>
    /// Current state of client socket.
    /// </summary>
    private LocalConnectionState _clientState = LocalConnectionState.Stopped;
    /// <summary>
    /// Current state of server socket.
    /// </summary>
    private LocalConnectionState _serverState = LocalConnectionState.Stopped;

    private CancellationTokenSource _shutdownSource;
    private int _port;

    public static bool IsServer;

    public static SessionManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
        networkManager.ServerManager.OnRemoteConnectionState += ServerManager_OnRemoteConnectionState;
        networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;

        sceneManager.OnLoadEnd += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        networkManager.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
        networkManager.ServerManager.OnRemoteConnectionState -= ServerManager_OnRemoteConnectionState;
        networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;

        if (_shutdownSource != null)
        {
            _shutdownSource.Cancel();
            _shutdownSource.Dispose();
        }
    }

    public void StartServerFlow()
    {
        IsServer = true;
        _port = port;
        networkManager.ServerManager.StartConnection(port);
    }

    public void StartServerFlow(int port)
    {
        IsServer = true;
        _port = port;
        networkManager.ServerManager.StartConnection((ushort)port);
    }

    public async void StartClientFlow()
    {
        IsServer= false;
        await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(lobbyScene.BuildIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void CreateSession()
    {
    }

    public void JoinSession()
    {
        networkManager.ClientManager.StartConnection(address, port);
    }

    public void JoinSession(string address, int port)
    {
        networkManager.ClientManager.StartConnection(address, (ushort)port);
    }

    private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
    {
        Debug.Log($"Server connection state changed: {obj}");
        _serverState = obj.ConnectionState;
        if (obj.ConnectionState == LocalConnectionState.Started)
        {
            sceneManager.LoadGlobalScenes(new SceneLoadData(redRoomScene.Name));
        }
    }

    private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
    {
        Debug.Log($"Client connection state changed: {obj}");
        _clientState = obj.ConnectionState;
        if (obj.ConnectionState == LocalConnectionState.Started)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(lobbyScene.BuildIndex);
        }
    }

    private void OnSceneLoaded(SceneLoadEndEventArgs obj)
    {
        Debug.Log("Scene loaded!");
        MatchmakerCommunication.Instance.SendServerReady(_port).Forget();
    }

    private void ServerManager_OnRemoteConnectionState(FishNet.Connection.NetworkConnection arg1, RemoteConnectionStateArgs arg2)
    {
        Debug.Log($"Client did something, clients remaining: {networkManager.ServerManager.Clients.Count}!");
        if (arg2.ConnectionState == RemoteConnectionState.Stopped)
        {
            Debug.Log($"Client left, clients remaining: {networkManager.ServerManager.Clients.Count}!");
            if (!CheckIfClientsActive())
            {
                Debug.Log("All clients left!");
                if (_shutdownSource != null)
                {
                    _shutdownSource.Cancel();
                    _shutdownSource.Dispose();
                }
                _shutdownSource = new CancellationTokenSource();
                CountdownToServerShutdown(_shutdownSource.Token).Forget();
            }
        }
        if (arg2.ConnectionState == RemoteConnectionState.Started)
        {
            _shutdownSource?.Cancel();
        }
    }

    private bool CheckIfClientsActive()
    {
        var clientsActive = false;
        foreach (var kvp in networkManager.ServerManager.Clients)
        {
            if (kvp.Value.IsActive)
            {
                clientsActive = true;
                break;
            }
        }
        return clientsActive;
    }

    private async UniTask CountdownToServerShutdown(CancellationToken cancellationToken)
    {
        try
        {
            await UniTask.Delay((int)(serverTimeToLiveInSeconds * 1000), cancellationToken: cancellationToken, ignoreTimeScale: true);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Shutdown canceled");
            return;
        }
        if (CheckIfClientsActive())
            return;
        networkManager.ServerManager.StopConnection(false);
        if (Application.isEditor)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        Application.Quit();
    }
}
