using UnityEngine;
using UnityEngine.InputSystem;

public class Quit : MonoBehaviour
{
    public InputActionReference action;
    void Start()
    {
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            ShutDown();
        };            
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            ShutDown();
    }

    void ShutDown()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif   
    }
}
