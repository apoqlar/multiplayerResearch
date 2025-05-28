using UnityEngine;
using Cysharp.Threading.Tasks;

public class LobbyController : MonoBehaviour
{
    public void JoinGameButton()
    {
        JoinGame().Forget();
    }

    public void StartServerButton()
    {
        StartAndJoinServer().Forget();
    }

    private async UniTask JoinGame()
    {
        var availableServers = await MatchmakerCommunication.Instance.GetAvailableServers();
        foreach (var availableServer in availableServers)
        {
            Debug.Log($"{availableServer.address} {availableServer.port}");
        }
        if (availableServers.Length > 0)
            SessionManager.Instance.JoinSession(availableServers[0].address, availableServers[0].port);
    }

    private async UniTask StartAndJoinServer()
    {
        var serverData = await MatchmakerCommunication.Instance.StartServer();
        SessionManager.Instance.JoinSession(serverData.address, serverData.port);
    }
}
