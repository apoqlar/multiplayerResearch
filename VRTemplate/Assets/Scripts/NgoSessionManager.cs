using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Multiplayer;
using UnityEditor.PackageManager;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NgoSessionManager : MonoBehaviour
{
    [SerializeField]
    private SceneReference lobbyScene;

    [SerializeField]
    private SceneReference redRoomScene;

    private string _profileName;
    private string _sessionName;
    private int _maxPlayers = 10;
    private ConnectionState _state = ConnectionState.Disconnected;
    private ISession _session;
    private NetworkManager m_NetworkManager;

    private enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected,
    }

    private async void Awake()
    {
        m_NetworkManager = GetComponent<NetworkManager>();
        m_NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        m_NetworkManager.OnSessionOwnerPromoted += OnSessionOwnerPromoted;
        await UnityServices.InitializeAsync();
        UnityEngine.SceneManagement.SceneManager.LoadScene(lobbyScene.BuildIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    private void OnSessionOwnerPromoted(ulong sessionOwnerPromoted)
    {
        if (m_NetworkManager.LocalClient.IsSessionOwner)
        {
            Debug.Log($"Client_{m_NetworkManager.LocalClientId} is the session owner!");
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        if (m_NetworkManager.LocalClientId == clientId)
        {
            m_NetworkManager.SceneManager.OnLoadComplete += OnLoadComplete;
            Debug.Log($"Client_{clientId} is connected and can spawn {nameof(NetworkObject)}s.");
        }
    }

    private void OnGUI()
    {
        if (_state == ConnectionState.Connected)
            return;

        GUI.enabled = _state != ConnectionState.Connecting;

        using (new GUILayout.HorizontalScope(GUILayout.Width(250)))
        {
            GUILayout.Label("Profile Name", GUILayout.Width(100));
            _profileName = GUILayout.TextField(_profileName);
        }

        using (new GUILayout.HorizontalScope(GUILayout.Width(250)))
        {
            GUILayout.Label("Session Name", GUILayout.Width(100));
            _sessionName = GUILayout.TextField(_sessionName);
        }

        GUI.enabled = GUI.enabled && !string.IsNullOrEmpty(_profileName) && !string.IsNullOrEmpty(_sessionName);

        if (GUILayout.Button("Create or Join Session"))
        {
            CreateOrJoinSessionAndScene().Forget();
        }
    }

    private void OnDestroy()
    {
        _session?.LeaveAsync();
        if (m_NetworkManager && m_NetworkManager.SceneManager != null)
            m_NetworkManager.SceneManager.OnLoadComplete -= OnLoadComplete;
    }

    private async UniTask CreateOrJoinSessionAsync()
    {
        _state = ConnectionState.Connecting;

        try
        {
            AuthenticationService.Instance.SwitchProfile(_profileName);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            var options = new SessionOptions()
            {
                Name = _sessionName,
                MaxPlayers = _maxPlayers
            }.WithDistributedAuthorityNetwork();
            _session = await MultiplayerService.Instance.CreateOrJoinSessionAsync(_sessionName, options);

            _state = ConnectionState.Connected;
        }
        catch (Exception e)
        {
            _state = ConnectionState.Disconnected;
            Debug.LogException(e);
        }
    }

    private async UniTask CreateOrJoinSessionAndScene()
    {
        _state = ConnectionState.Connecting;
        try
        {
            AuthenticationService.Instance.SwitchProfile(_profileName);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            QuerySessionsOptions options = new QuerySessionsOptions
            { 
            };
            var sessions = await MultiplayerService.Instance.QuerySessionsAsync(options);
            var sessionExists = false;
            foreach (var  session in sessions.Sessions)
            {
                Debug.Log($"Existing session: {session.Name}");
                if (session.Name == _sessionName)
                    sessionExists = true;
            }
            if (sessionExists)
            {
                Debug.Log($"Session {_sessionName} exists, joining");
                await JoinSessionAndScene();
            }
            else
            {
                Debug.Log($"Session does not exist, create and join");
                await CreateSessionAndScene();
            }

            await SceneManager.UnloadSceneAsync(lobbyScene.Name);
        }
        catch (Exception e)
        {
            _state = ConnectionState.Disconnected;
            Debug.LogException(e);
        }
    }

    private async UniTask CreateSessionAndScene()
    {
        try
        {
            var options = new SessionOptions()
            {
                Name = _sessionName,
                MaxPlayers = _maxPlayers
            }.WithDistributedAuthorityNetwork();

            _session = await MultiplayerService.Instance.CreateOrJoinSessionAsync(_sessionName, options);
            _state = ConnectionState.Connected;
            await UniTask.WaitUntil(() => m_NetworkManager.LocalClient.IsSessionOwner);
            m_NetworkManager.SceneManager.LoadScene(redRoomScene.Name, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
        catch (SessionException e)
        {
            Debug.LogException(e);
            _state = ConnectionState.Disconnected;
        }
    }

    private async UniTask JoinSessionAndScene()
    {
        try
        {
            var options = new SessionOptions()
            {
                Name = _sessionName,
                MaxPlayers = _maxPlayers
            }.WithDistributedAuthorityNetwork();
            await MultiplayerService.Instance.CreateOrJoinSessionAsync(_sessionName, options);
        }
        catch (SessionException e)
        {
            Debug.LogException(e);
            _state = ConnectionState.Disconnected;
        }
    }

    private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        //var spawnPosition = new Vector3(0, 1, 0); // example position
        //var playerPrefab = NetworkManager.Singleton.NetworkConfig.PlayerPrefab;
        //var playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        //playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }

}