using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking;

public class MatchmakerCommunication : MonoBehaviour
{
    [SerializeField]
    private string matchmakerUrl = "http://localhost";

    [SerializeField]
    private int matchmakerPort = 5000;

    [SerializeField]
    private string startServerEndpoint = "/unity/start";

    [SerializeField]
    private string serverReadyEndpoint = "/unity/ready";

    [SerializeField]
    private string availableServersEndpoint = "/unity/servers";

    public static MatchmakerCommunication Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public async UniTask<ServerData> StartServer()
    {
        var url = $"{matchmakerUrl}:{matchmakerPort}{startServerEndpoint}";

        var request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Server response: " + request.downloadHandler.text);
            return JsonConvert.DeserializeObject<ServerData>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            return new ServerData();
        }
    }

    public async UniTask SendServerReady(int port)
    {
        var url = $"{matchmakerUrl}:{matchmakerPort}{serverReadyEndpoint}?port={port}";

        var request = UnityWebRequest.Get(url);
        await request.SendWebRequest();
    }

    public async UniTask<ServerData[]> GetAvailableServers()
    {
        var url = $"{matchmakerUrl}:{matchmakerPort}{availableServersEndpoint}";

        var request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Server response: " + request.downloadHandler.text);
            return JsonConvert.DeserializeObject<ServerData[]>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            return Array.Empty<ServerData>();
        }
    }
}
