using FishNet.Component.Ownership;
using FishNet.Object;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NetworkedGrab : MonoBehaviour
{
    [SerializeField]
    private XRGrabInteractable grabInteractable;

    [SerializeField]
    private NetworkObject networkObject;

    [SerializeField]
    private PredictedOwner predictedOwner;

    private void Start()
    {
        Debug.Log("Network grab started");
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log($"Was owner: {networkObject.IsOwner}");
        Debug.Log("Getting ownership");
        predictedOwner.TakeOwnership(true);
        Debug.Log($"Is owner: {networkObject.IsOwner}");

    }
}
