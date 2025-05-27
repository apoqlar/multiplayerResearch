using FishNet.Component.Ownership;
using FishNet.Object;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NetworkedGrab : NetworkBehaviour
{
    [SerializeField]
    private XRGrabInteractable grabInteractable;

    [SerializeField]
    private NetworkObject networkObject;

    [SerializeField]
    private PredictedOwner predictedOwner;

    [SerializeField]
    private Rigidbody networkedBody;

    private void Start()
    {
        Debug.Log("Network grab started");
        predictedOwner.TakeOwnership(true);
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExit);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExit);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log($"Was owner: {networkObject.IsOwner}");
        Debug.Log("Getting ownership");
        predictedOwner.TakeOwnership(true);
        SyncProperties(networkedBody.isKinematic, networkedBody.useGravity);
        Debug.Log($"Is owner: {networkObject.IsOwner}");
    }

    private void OnSelectExit(SelectExitEventArgs args)
    {
        SyncProperties(networkedBody.isKinematic, networkedBody.useGravity);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SyncProperties(bool isKinematic, bool useGravity)
    {
        networkedBody.isKinematic = isKinematic;
        networkedBody.useGravity = useGravity;
    }
}
