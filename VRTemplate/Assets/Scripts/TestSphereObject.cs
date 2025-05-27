using FishNet.Object;
using UnityEngine;
using Zenject;

public class TestSphereObject : NetworkBehaviour
{
    [SerializeField]
    private bool sendMessage;


    private TestInjectable _injectable;

    [Inject]
    private void Construct(TestInjectable injectable)
    {
        Debug.Log("Constructing sphere");
        _injectable = injectable;
    }

    private void Update()
    {
        if (sendMessage)
        {
            BroadcastMessage();
            sendMessage = false;
        }
    }

    public void BroadcastMessage()
    {
        Debug.Log($"Broadcasting message: {_injectable.testMessage}");
        SendMessage_Server(_injectable.testMessage);
    }

    [ServerRpc]
    public void SendMessage_Server(string messageToServer)
    {
        Debug.Log($"Got message on server: {messageToServer}");
        SendMessageToClient(messageToServer);
    }

    [ObserversRpc]
    public void SendMessageToClient(string messageFromServer) 
    {
        Debug.Log($"Got message on client: {messageFromServer}");
    }

    public class TestSphereObjectFactory : PlaceholderFactory<TestSphereObject>
    {

    }
}
