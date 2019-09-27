using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Collider _collider;

    public Companion companion;
    public float runSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    private static readonly int Speed = Animator.StringToHash("speed");
    private bool _isHeldBack = false;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.ResetPath();
    }

    void Update()
    {
        float translation = Input.GetAxis("Vertical") * runSpeed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
            
        bool isMovePressed = Math.Abs(translation) > 0.01f || Math.Abs(rotation) > 0.01f;

        if (_navMeshAgent.enabled)
        {
            if (isMovePressed)
            {
                MovePlayer(translation, rotation);
            }
            else
            {
                StopPlayer();
            }
        }

        
        _animator.SetFloat(Speed, _navMeshAgent.speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            AttemptToUse(other);
        }
    }

    private void AttemptToUse(Collider other)
    {
        Debug.Log("attempting to use");
        Usable usable = other.gameObject.GetComponent<Usable>();
        if (usable)
        {
            usable.Use();
        }
    }

    private void StopPlayer()
    {
        _navMeshAgent.ResetPath();
        _navMeshAgent.speed = 0.0f;
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.isStopped = true;
    }

    public void ToggleHeldBack(bool isHeldBack)
    {
        this._isHeldBack = isHeldBack;
    }

    private void MovePlayer(float translation, float rotation)
    {
        Vector3 velocityForward = transform.forward * translation;
        Vector3 velocitySideways = transform.right * rotation;
        Vector3 resultantVelocity = velocityForward + velocitySideways;
        Vector3 destination = resultantVelocity + transform.position;

        if (_isHeldBack)
        {
            Vector3 destFlat = new Vector3(destination.x, 0, destination.z).normalized;
            Vector3 companionDirection = (companion.transform.position - transform.position).normalized;
            Vector3 companionFlat = new Vector3(companionDirection.x, 0, companionDirection.z);
            float dot = Vector3.Dot(destFlat, companionFlat);
            if (Vector3.Dot(destFlat, companionFlat) < -0.5f)
            {
                Debug.Log(dot);
                return;
            }
        }
        
        _navMeshAgent.speed = resultantVelocity.magnitude;
        _navMeshAgent.velocity = resultantVelocity;
        _navMeshAgent.updateRotation = translation > 0.0f || Math.Abs(rotation) > 0.0f;
        _navMeshAgent.SetDestination(destination);
    }
}
