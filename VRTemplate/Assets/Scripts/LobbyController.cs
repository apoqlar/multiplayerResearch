using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public void JoinGame()
    {
        SessionManager.Instance.JoinSession();
    }
}
