using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AutoUsable : MonoBehaviour
{
    public bool isOneTime = false;
    public UnityEvent onStepOn;
    public UnityEvent onStepOff;
    public AudioClip audioClip;

    private MeshRenderer _meshRenderer;
    private BoxCollider _boxCollider;
    private AudioSource _audioSource;
    private bool _hasBeenUsed = false;
    private bool _canBeUsed = true;

    private readonly int shaderColor = Shader.PropertyToID("Color_C427DAE8");

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _audioSource = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void KillSwitch()
    {
        _canBeUsed = false;
        _hasBeenUsed = true;
        isOneTime = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((!isOneTime || !_hasBeenUsed) && _canBeUsed)
        {
            if (other.gameObject.layer == Layers.Player || other.gameObject.layer == Layers.Companion || other.gameObject.layer == Layers.Usable)
            {
                onStepOn?.Invoke();
                _audioSource.PlayOneShot(audioClip);
                _hasBeenUsed = true;
                _canBeUsed = false;

                if (_meshRenderer)
                {
                    _meshRenderer.material.SetColor(shaderColor, Color.red);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isOneTime || !_hasBeenUsed)
        {
            if (other.gameObject.layer == Layers.Player || other.gameObject.layer == Layers.Companion || other.gameObject.layer == Layers.Usable)
            {
                onStepOff?.Invoke();
                _canBeUsed = true;
                
                if (_meshRenderer)
                {
                    _meshRenderer.material.SetColor(shaderColor, Color.yellow);
                }
            }
        }
    }
}
