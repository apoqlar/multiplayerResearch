using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleAvatar : NetworkBehaviour
{
    [SerializeField]
    private GameObject leftHand;

    [SerializeField] 
    private GameObject rightHand;

    [SerializeField]
    private GameObject head;

    [SerializeField]
    private InputActionReference leftHandPosition;

    [SerializeField]
    private InputActionReference leftHandRotation;

    [SerializeField]
    private InputActionReference rightHandPosition;

    [SerializeField]
    private InputActionReference rightHandRotation;

    [SerializeField]
    private InputActionReference headPosition;

    [SerializeField]
    private InputActionReference headRotation;


    public void Start()
    {
    }

    public void Update()
    {
        if (!NetworkObject.IsOwner)
            return;
        leftHand.transform.position = leftHandPosition.action.ReadValue<Vector3>();
        leftHand.transform.rotation = leftHandRotation.action.ReadValue<Quaternion>();
        rightHand.transform.position = rightHandPosition.action.ReadValue<Vector3>();
        rightHand.transform.rotation = rightHandRotation.action.ReadValue<Quaternion>();
        head.transform.position = headPosition.action.ReadValue<Vector3>();
        head.transform.rotation = headRotation.action.ReadValue<Quaternion>();
    }
}
