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

    public float runSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    private static readonly int Speed = Animator.StringToHash("speed");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.ResetPath();
    }

    // Update is called once per frame
    void FixedUpdate()
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

    private void StopPlayer()
    {
        _navMeshAgent.ResetPath();
        _navMeshAgent.speed = 0.0f;
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.isStopped = true;
    }

    private void MovePlayer(float translation, float rotation)
    {
        Vector3 velocityForward = transform.forward * translation;
        Vector3 velocitySideways = transform.right * rotation;
        Vector3 resultantVelocity = velocityForward + velocitySideways;
        _navMeshAgent.speed = resultantVelocity.magnitude;
        _navMeshAgent.velocity = resultantVelocity;
        _navMeshAgent.updateRotation = translation > 0.0f || Math.Abs(rotation) > 0.0f;
        _navMeshAgent.SetDestination(resultantVelocity + transform.position);
    }
}
