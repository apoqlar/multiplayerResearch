using FishNet.Component.Ownership;
using FishNet.Object;
using FishNet.Object.Prediction;
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
        //predictedOwner.TakeOwnership(true);
        //grabInteractable.selectEntered.AddListener(OnSelectEntered);
        //grabInteractable.selectExited.AddListener(OnSelectExit);
    }

    private void OnDestroy()
    {
        //grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        //grabInteractable.selectExited.RemoveListener(OnSelectExit);
    }

    public void OnSelectEntering()
    {
        predictedOwner.TakeOwnership(true);
    }

    public void OnSelectExitting()
    {

    }

    public void OnSelectEntered()
    {
        SyncProperties(networkedBody.isKinematic, networkedBody.useGravity);
    }

    public void OnSelectExited()
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
