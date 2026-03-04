using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HandController : MonoBehaviour
{
    [SerializeField] private Hand hand;
    HandController otherHand = null;

    public InputActionReference gripAction;
    public InputActionReference triggerAction;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference grabAction;
    public bool doubleRotationEnabled = false;
    public InputActionReference toggleRotationAction;
    bool grabbing = false;
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private void Start()
    {
        toggleRotationAction.action.performed += ctx => doubleRotationEnabled = !doubleRotationEnabled;

        // Find the other hand
        foreach(HandController c in transform.parent.GetComponentsInChildren<HandController>())
        {
            if (c != this)
                otherHand = c;
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation; 
    }

    void OnEnable()
    {
        grabAction.action.Enable();
        toggleRotationAction.action.Enable();
        gripAction.action.Enable();
        triggerAction.action.Enable();
    }

    void OnDisable()
    {
        grabAction.action.Disable();
        toggleRotationAction.action.Disable();
        gripAction.action.Disable();
        triggerAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        hand.SetGrip(gripAction.action.ReadValue<float>());
        hand.SetTrigger(triggerAction.action.ReadValue<float>());
        // Grabbing(); Transferred to using XRI Grab Interactable, because this doesn't work well with gravity.
    }

    private void Grabbing()
    {
        grabbing = grabAction.action.IsPressed();

        Vector3 deltaPos = transform.position - lastPosition;
        Quaternion deltaRot = transform.rotation * Quaternion.Inverse(lastRotation);

        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject)
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : (otherHand ? otherHand.grabbedObject : null);

            if (grabbedObject)
            {
                // Change these to add the delta position and rotation instead
                // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here
                Transform objectToMove = grabbedObject.parent ? grabbedObject.parent : grabbedObject;
                Rigidbody rigidbody = objectToMove.GetComponent<Rigidbody>();
                if(rigidbody != null && !rigidbody.isKinematic)
                {
                    rigidbody.isKinematic = true;
                    rigidbody.useGravity = false;
                }
                objectToMove.position += deltaPos;
                Vector3 offset = objectToMove.position - transform.position;
                if (doubleRotationEnabled)
                {
                    deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
                    deltaRot = Quaternion.AngleAxis(angle * 2f, axis);
                }
                    
                objectToMove.position = transform.position + (deltaRot * offset);
                objectToMove.rotation = deltaRot * objectToMove.rotation;
            }
        }
        // If let go of button, release object
        else if (grabbedObject)
        {
            Transform objectToMove = grabbedObject.parent ? grabbedObject.parent : grabbedObject;
            Rigidbody rigidbody = objectToMove.GetComponent<Rigidbody>();
            if(rigidbody != null)
            {
                rigidbody.isKinematic = false; // Re-enable physics when releasing
                rigidbody.useGravity = true;
            }
            grabbedObject = null;
        }
        // Should save the current position and rotation here
        lastPosition = transform.position;
        lastRotation = transform.rotation; 
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
    }

    
}
