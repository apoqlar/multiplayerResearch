using Eflatun.SceneReference;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Transporting;
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

    /// <summary>
    /// Current state of client socket.
    /// </summary>
    private LocalConnectionState _clientState = LocalConnectionState.Stopped;
    /// <summary>
    /// Current state of server socket.
    /// </summary>
    private LocalConnectionState _serverState = LocalConnectionState.Stopped;

    public static SessionManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
        networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;

        sceneManager.OnLoadEnd += OnSceneLoaded;
    }

    public void StartServerFlow()
    {
        networkManager.ServerManager.StartConnection(port);
    }

    public async void StartClientFlow()
    {
        await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(lobbyScene.BuildIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void CreateSession()
    {
    }

    public void JoinSession()
    {
        networkManager.ClientManager.StartConnection(address, port);
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
    }

}
