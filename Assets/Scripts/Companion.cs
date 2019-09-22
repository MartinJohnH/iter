using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.UIElements;

public class Companion : MonoBehaviour
{
    public Transform player;
    public Transform tetherAnchor;
    public bool isTethered = true;
    public float tetherRadius = 10.0f;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private LineRenderer _lineRenderer;
    private static readonly int Speed = Animator.StringToHash("speed");

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.ResetPath();

        _lineRenderer.positionCount = 2;
        RenderTether();
    }

    private void RenderTether()
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.SetPosition(0, tetherAnchor.position);
        _lineRenderer.SetPosition(1, player.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Fire2"))
        {
            if (IsCloseEnoughToTether())
            {
                ToggleTether();
            }
        }
        
        if (isTethered)
        {
            RenderTether();
            FollowPlayer();
        }
        else
        {
            _navMeshAgent.ResetPath();
            RenderTetheringRadius();
        }
        
        _animator.SetFloat(Speed, _navMeshAgent.velocity.sqrMagnitude);
    }

    private void RenderTetheringRadius()
    {
        float x;
        float y;
        float z;

        float angle = 20f;
        int segments = 50;

        _lineRenderer.positionCount = segments + 1;
        _lineRenderer.useWorldSpace = false;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * tetherRadius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * tetherRadius;

            _lineRenderer.SetPosition (i,new Vector3(x,0,z) );

            angle += (360f / segments);
        }
    }

    private void ToggleTether()
    {
        if (IsCloseEnoughToTether())
        {
            isTethered = !isTethered;
        }
    }

    private bool IsCloseEnoughToTether()
    {
        return Vector3.Distance(transform.position, player.position) < tetherRadius;
    }

    private void FollowPlayer()
    {
        if ((player.transform.position - transform.position).sqrMagnitude > 2.0f)
        {
            _navMeshAgent.SetDestination(player.transform.position);
            _navMeshAgent.updatePosition = true;
        }
        else
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.ResetPath();
        }
    }
}
