using UnityEngine;
using UnityEngine.InputSystem;
public class LightSwitch : MonoBehaviour
{
    public new Light light;
    public InputActionReference action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light = GetComponent<Light>();
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            LightToggle();
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle light color between blue and white when L key is pressed, 
        // or when the assigned input action is performed.
        // We have switched active Input handling to Input System package in Player Settings,
        // so using Input.GetKeyDown doesn't work.

        if (Keyboard.current.lKey.wasPressedThisFrame)
            LightToggle();
    }

    void LightToggle()
    {
        if (light.color == Color.blue)
            light.color = Color.white;
        else
            light.color = Color.blue;
    }
}
