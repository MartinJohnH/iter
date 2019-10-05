using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(NavMeshAgent))]
public class Companion : MonoBehaviour
{
    public Player player;
    public Transform tetherAnchor;
    public Material dissolveMaterial;
    public bool isTethered = true;
    public float tetherRadius = 5.0f;
    public float dissolveTimeLimit = 10.0f;
    public UnityEvent onDeath;
    public UnityEvent onDeathCanceled;

    private bool _isDying = false;
    private NavMeshAgent _navMeshAgent;
    private SkinnedMeshRenderer[] _meshRenderers;
    private Material[] defaultMaterials;
    private Animator _animator;
    private LineRenderer _lineRenderer;
    private static readonly int Speed = Animator.StringToHash("speed");
    private static readonly int Progress = Shader.PropertyToID("Vector1_47ABB1D2");

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        defaultMaterials = new Material[_meshRenderers.Length];

        GetComponent<Rigidbody>().isKinematic = true;
        _navMeshAgent.ResetPath();

        _lineRenderer.positionCount = 2;
        RenderTether();
        
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            defaultMaterials[i] = _meshRenderers[i].material;
        }
    }

    private void RenderTether()
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.SetPosition(0, tetherAnchor.position);
        _lineRenderer.SetPosition(1, player.tetherAnchor.position);
    }

    void Update()
    {
        
        if (Input.GetButtonDown("Fire2"))
        {
            if (IsCloseEnoughToTether())
            {
                ToggleTether();
            }
        }

        if (_navMeshAgent.enabled)
        {
            _lineRenderer.enabled = true;
            
            if (isTethered)
            {
                RenderTether();
                FollowPlayer();
                if (!IsCloseEnoughToTether())
                {
                    ToggleTether();
                }
            }
            else
            {
                _navMeshAgent.ResetPath();
                RenderTetheringRadius();
            }
        }
        else
        {
            _lineRenderer.enabled = false;
        }

        if (_animator)
        {
            _animator.SetFloat(Speed, _navMeshAgent.velocity.sqrMagnitude);
        }
    }

    private IEnumerator StartDissolve()
    {
        _isDying = true;
        
        float stepAmount = 0.01f;
        float stepTime = stepAmount * dissolveTimeLimit;
        float progress = 0.0f;
        
        foreach (SkinnedMeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.material = dissolveMaterial;
        }
        
        while (progress <= 1.0f)
        {
            foreach (SkinnedMeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.material.SetFloat(Progress, progress);
            }

            progress += stepAmount;

            yield return new WaitForSeconds(stepTime);
        }

        onDeath?.Invoke();
    }

    private void CancelDissolve()
    {
        _isDying = false;
        
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].material = defaultMaterials[i];
        }
        
        onDeathCanceled?.Invoke();
    }

    private void RenderTetheringRadius()
    {
        float x;
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
        if (!isTethered && IsCloseEnoughToTether())
        {
            isTethered = true;
            if (_isDying)
            {
                StopCoroutine(StartDissolve());
                CancelDissolve();
            }
        }
        else
        {
            isTethered = false;
            if (!_isDying)
            {
                StartCoroutine(StartDissolve());
            }
        }
    }

    private bool IsCloseEnoughToTether()
    {
        return Vector3.Distance(transform.position, player.transform.position) < tetherRadius;
    }

    private void FollowPlayer()
    {
        if ((player.transform.position - transform.position).sqrMagnitude > 5.0f)
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
