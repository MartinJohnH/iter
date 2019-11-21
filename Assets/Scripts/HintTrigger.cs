using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HintTrigger : MonoBehaviour
{
    public UnityEvent callback;
    
    private void OnTriggerEnter(Collider other)
    {
        callback?.Invoke();
    }
}
