using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GravityListener : MonoBehaviour
{
    public bool shouldRotate = false;
    public float rotationSpeed = 10.0f;
    
    private GravityController _gravityController;
    private Rigidbody _rigidbody;
    private Vector3 _gravity = Physics.gravity;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gravityController = FindObjectOfType<GravityController>();
        _gravityController.register(this);
    }

    private void OnDestroy()
    {
        _gravityController.unregister(this);
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_gravity * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void OnGravityChange(Vector3 newGravity)
    {
        _gravity = newGravity;
        
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        if (shouldRotate)
        {
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
            StartCoroutine(Rotate(newUp, newForward));
        }
        
        if (agent)
        {
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.updateUpAxis = true;
        }
    }

    private IEnumerator Rotate(Vector3 newUp, Vector3 newForward)
    {
        
        Quaternion rotation = Quaternion.LookRotation(newForward, newUp);
        
        while (!transform.rotation.Equals(rotation))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
            yield return new WaitForFixedUpdate();
        }
        
    }
}
