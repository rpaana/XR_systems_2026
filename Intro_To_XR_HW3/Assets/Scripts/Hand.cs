using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Hand : MonoBehaviour
{
    //Animation
    [SerializeField] private float animationSpeed;
    private Animator _animator;
    private float _gripTarget;
    private float _triggerTarget;
    private float _gripCurrent;
    private float _triggerCurrent;
    private const string AnimatorGripParam = "Grip";
    private const string AnimatorTriggerParam = "Trigger";
    private static readonly int Grip = Animator.StringToHash(AnimatorGripParam);
    private static readonly int Trigger = Animator.StringToHash(AnimatorTriggerParam);

    //Physics Movement
    [SerializeField] private GameObject followObject;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    private Transform _followTarget;
    private Rigidbody _rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Animation
        _animator = GetComponent<Animator>();

        // Physics Movement
        _followTarget = followObject.transform;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.mass = 20f;

        // Teleport hands
        _rigidbody.position = _followTarget.position;
        _rigidbody.rotation = _followTarget.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }

    void FixedUpdate()
    {
        PhysicsMove();
    }

    private void PhysicsMove()
    {
        // Position
        var rotatedPositionOffset = _rigidbody.rotation * positionOffset;
        var positionWithOffset = _followTarget.position + rotatedPositionOffset;
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        _rigidbody.linearVelocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);
        // var positionWithOffset = _followTarget.position + (_followTarget.rotation * positionOffset);
        // _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, positionWithOffset, followSpeed * Time.fixedDeltaTime));

        // Rotation
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var rotationDifference = rotationWithOffset * Quaternion.Inverse(_rigidbody.rotation);
        rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);
        _rigidbody.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
        // _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, rotationWithOffset, rotateSpeed * Time.fixedDeltaTime));
    }

    internal void SetGrip(float v)
    {
        _gripTarget = v;

    }

    internal void SetTrigger(float v)
    {
        _triggerTarget = v;
    }

    void AnimateHand()
    {
        if (_gripCurrent != _gripTarget)
        {
            _gripCurrent = Mathf.MoveTowards(_gripCurrent, _gripTarget, Time.deltaTime * animationSpeed);
            _animator.SetFloat(Grip, _gripCurrent);
        }
        if (_triggerCurrent != _triggerTarget)
        {
            _triggerCurrent = Mathf.MoveTowards(_triggerCurrent, _triggerTarget, Time.deltaTime * animationSpeed);
            _animator.SetFloat(Trigger, _triggerCurrent);
        }
    }

}
