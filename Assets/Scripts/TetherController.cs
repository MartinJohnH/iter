using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherController : MonoBehaviour
{
    public Transform playerAnchor;
    public Transform companionAnchor;
    public float followSpeed = 100.0f;

    private ParticleSystem _particleSystem;

    private bool _movingTo = true;
    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.transform.position = playerAnchor.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAnchor && companionAnchor)
        {
            if (_movingTo)
            {
                transform.position = Vector3.MoveTowards(transform.position, companionAnchor.position,
                    Time.deltaTime * followSpeed);
                
                if (Vector3.Distance(transform.position, companionAnchor.position) < 0.001f)
                {
                    _movingTo = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, playerAnchor.position,
                    Time.deltaTime * followSpeed);
                
                if (Vector3.Distance(transform.position, playerAnchor.position) < 0.001f)
                {
                    _movingTo = true;
                }
            }
        }
    }
}
