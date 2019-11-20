using System;
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
    public Material dissolveMaterial;
    public bool isTethered = true;
    public float tetherRadius = 5.0f;
    public TetherController tether;
    public float dissolveTimeLimit = 10.0f;
    public UnityEvent onDeath;
    public UnityEvent onDeathTick;
    public UnityEvent onDeathCanceled;
    public AudioSource clockSound;
    public AudioSource stepSound;

    private IEnumerator _dissolveCoroutine;
    private IEnumerator _tickCoroutine;
    private int _framesBeforeLightTouch = 0;
    private bool _isDying = false;
    private NavMeshAgent _navMeshAgent;
    private SkinnedMeshRenderer[] _meshRenderers;
    private Material[] _defaultMaterials;
    private Animator _animator;
    private LineRenderer _lineRenderer;
    private static readonly int Speed = Animator.StringToHash("speed");
    private static readonly int Progress = Shader.PropertyToID("Vector1_47ABB1D2");
    private static readonly int IsTethered = Animator.StringToHash("isTethered");

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        _defaultMaterials = new Material[_meshRenderers.Length];

        GetComponent<Rigidbody>().isKinematic = true;
        _navMeshAgent.ResetPath();

        _lineRenderer.positionCount = 2;
        tether.enabled = true;
        
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _defaultMaterials[i] = _meshRenderers[i].material;
        }
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
            
            
            if (isTethered)
            {
                _lineRenderer.enabled = false;
                FollowPlayer();
                if (!IsCloseEnoughToTether())
                {
                    ToggleTether();
                }
            }
            else
            {
                _lineRenderer.enabled = true;
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

    private IEnumerator MakeTick()
    {
        float time = dissolveTimeLimit;
        while (time >= 0.0f)
        {
            onDeathTick?.Invoke();
            time -= 1.0f;
            yield return new WaitForSeconds(1.0f);
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
            dissolveMaterial.SetFloat(Progress, progress);
            progress += stepAmount;
            yield return new WaitForSeconds(stepTime);
        }

        clockSound.Stop();
        MusicController.GetInstance().TransitionTo(MusicController.Variation.A_Lowpass);
        onDeath?.Invoke();
    }

    private void CancelDissolve()
    {
        StopCoroutine(_dissolveCoroutine);
        StopCoroutine(_tickCoroutine);
        clockSound.Stop();
        MusicController.GetInstance().ToggleLowpass(false);
        
        _isDying = false;
        
        dissolveMaterial.SetFloat(Progress, 0.0f);
        
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].material = _defaultMaterials[i];
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
            tether.enabled = true;
            if (_isDying)
            {
                CancelDissolve();
            }
        }
        else
        {
            isTethered = false;
            tether.enabled = false;
            if (!_isDying)
            {
                _dissolveCoroutine = StartDissolve();
                _tickCoroutine = MakeTick();
                clockSound.Play();
                MusicController.GetInstance().ToggleLowpass(true);
                StartCoroutine(_dissolveCoroutine);
                StartCoroutine(_tickCoroutine);
            }
        }
        _animator.SetBool(IsTethered, isTethered);
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
    
    private void OnTriggerStay(Collider other)
    {
        int layer = other.gameObject.layer;
        _framesBeforeLightTouch = (++_framesBeforeLightTouch % 20);

        if (layer == Layers.Light && isTethered && _framesBeforeLightTouch == 0)
        {
            ToggleTether();
            _framesBeforeLightTouch = 0;
        }
    }

    public void OnStep()
    {
        stepSound.Play();
    }
}
