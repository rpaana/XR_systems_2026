using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AttachSwapper : MonoBehaviour
{
    [SerializeField] private Transform handlePoint;
    [SerializeField] private Transform socketPoint;
    private XRGrabInteractable interactable;

    void Awake() => interactable = GetComponent<XRGrabInteractable>();

    // Call this from the Socket's "Select Entered" event
    public void SetSocketAttach() => interactable.attachTransform = socketPoint;

    // Call this from the Socket's "Select Exited" event
    public void SetHandleAttach() => interactable.attachTransform = handlePoint;
}
