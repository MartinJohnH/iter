using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoUsable : MonoBehaviour
{
    public UnityEvent onStepOn;
    public UnityEvent onStepOff;

    private BoxCollider _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Player || other.gameObject.layer == Layers.Companion)
        {
            onStepOn?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layers.Player || other.gameObject.layer == Layers.Companion)
        {
            onStepOff?.Invoke();
        }
    }
}
