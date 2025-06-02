using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NetworkedGrabNGO : NetworkBehaviour
{
    [SerializeField]
    private XRGrabInteractable grabInteractable;

    [SerializeField]
    private NetworkObject networkObject;

    [SerializeField]
    private Rigidbody networkedBody;

    public void OnSelectEntering()
    {
        NetworkObject.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }

    public void OnSelectExitting()
    {

    }

    public void OnSelectEntered()
    {
        SyncPropertiesRpc(networkedBody.isKinematic, networkedBody.useGravity);
    }

    public void OnSelectExited()
    {
        SyncPropertiesRpc(networkedBody.isKinematic, networkedBody.useGravity);
    }

    [Rpc(SendTo.NotMe)]
    private void SyncPropertiesRpc(bool isKinematic, bool useGravity)
    {
        networkedBody.isKinematic = isKinematic;
        networkedBody.useGravity = useGravity;
    }
}
