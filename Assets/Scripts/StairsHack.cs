using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StairsHack : MonoBehaviour
{
    public OffMeshLink offMeshLink;

    private void Start()
    {
        offMeshLink.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == Layers.Usable)
        {
            offMeshLink.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layers.Usable)
        {
            offMeshLink.enabled = false;
        }
    }
}
