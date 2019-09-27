using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Usable : MonoBehaviour
{
    public List<UnityEvent> onUsed = new List<UnityEvent>();
    public List<UnityEvent> afterUsed = new List<UnityEvent>();

    private BoxCollider _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }

    public void Use()
    {
        foreach (UnityEvent unityEvent in onUsed)
        {
            unityEvent.Invoke();
        }

        foreach (UnityEvent unityEvent in afterUsed)
        {
            unityEvent.Invoke();
        }
    }

}
