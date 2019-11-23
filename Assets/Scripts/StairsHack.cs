using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StairsHack : MonoBehaviour
{
    public NavMeshLink offMeshLink;
    public Material successMaterial;
    public GameObject cube;
    private AudioSource _audioSource;

    private void Start()
    {
        offMeshLink.enabled = false;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Usable)
        {
            offMeshLink.enabled = true;
            _audioSource.Play();
            cube.GetComponent<MeshRenderer>().material = successMaterial;
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
