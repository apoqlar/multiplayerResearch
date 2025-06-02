using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NetworkedGrabInteractableNGO : XRGrabInteractable
{
    [SerializeField]
    private NetworkedGrabNGO networkComponent;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        networkComponent.OnSelectEntering();
        base.OnSelectEntering(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        networkComponent.OnSelectExitting();
        base.OnSelectExiting(args);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        networkComponent.OnSelectEntered();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        networkComponent.OnSelectExited();
    }
}
