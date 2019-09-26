using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Any object that uses this must freeze their rotation in all axes
 */
[RequireComponent(typeof(Rigidbody))]
public class GravityListener : MonoBehaviour
{
    public float rotationSpeed = 180.0f;
    public int rotationDelay = 60;
    
    private GravityController _gravityController;
    private Rigidbody _rigidbody;
    private Vector3 _gravity = Physics.gravity;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gravityController = FindObjectOfType<GravityController>();
        _gravityController.register(this);

        _rigidbody.freezeRotation = true;
    }

    private void OnDestroy()
    {
        _gravityController.unregister(this);
    }

    public void OnGravityChange(Vector3 newGravity)
    {
        _gravity = newGravity;
        
        Vector3 normalizedGravity = newGravity.normalized;
        Vector3 newUp = normalizedGravity * -1.0f;
        Vector3 newForward = transform.forward;
        if (normalizedGravity.Equals(transform.forward))
        {
            newForward = transform.up;
        } else if (normalizedGravity.Equals(transform.forward * -1.0f))
        {
            newForward = transform.up * -1.0f;
        }
        StartCoroutine(RotateAndFall(newUp, newForward));
        
    }

    private IEnumerator RotateAndFall(Vector3 newUp, Vector3 newForward)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.enabled = false;
        }
        
        Quaternion rotation = Quaternion.LookRotation(newForward, newUp);

        int currentFrame = 0;
        
        while (currentFrame++ < rotationDelay)
        {
            yield return new WaitForFixedUpdate();
        }

        while (!transform.rotation.Equals(rotation))
        {
            _rigidbody.velocity = _rigidbody.velocity * 0.5f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        while (!IsGrounded())
        {
            yield return new WaitForFixedUpdate();
        }

        
        if (agent)
        {
            agent.enabled = true;
        }
        
    }

    private bool IsGrounded()
    {
        Vector3 down = _gravity.normalized;
        Vector3 velocity = _rigidbody.velocity;
        return Math.Abs(new Vector3(down.x * velocity.x, down.y * velocity.y, down.z * velocity.z).sqrMagnitude) < 0.0001f;
    }
}
