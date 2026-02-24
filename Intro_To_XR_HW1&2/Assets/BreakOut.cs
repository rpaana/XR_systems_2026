using UnityEngine;
using UnityEngine.InputSystem;

// This script will move the player between an external viewing point and the room.
// Pressing the button for the first time will move the player to the external viewing point.
// After that, pressing the button alternates the player's position between the two locations.
public class BreakOut : MonoBehaviour
{
    public Transform externalViewPoint;
    public Transform roomViewPoint;
    public bool isExternalView = false;
    public InputActionReference action;

    void Start()
    {
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            BreakOutToggle();
        };   
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the secondary button on the right Oculus controller is pressed
        if (Keyboard.current.bKey.wasPressedThisFrame)
            BreakOutToggle();
    }

    void BreakOutToggle()
    {
        isExternalView = !isExternalView;
        transform.position = isExternalView ? externalViewPoint.position : roomViewPoint.position;
        // Set rotation so that the player faces the center of the room when in external view
        // and faces the instruction wall when in the room.
        transform.rotation = isExternalView ? Quaternion.Euler(0, 45, 0) : Quaternion.Euler(0, 180, 0);
    }
}
