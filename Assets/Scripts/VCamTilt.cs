using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class VCamTilt : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float maxTilt = 3.0f;

    [Range(0.0f, 10.0f)]
    public float maxPan = 2.0f;
    
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineComposer _composer;
    private float _mouseY = 0.0f;
    private float _mouseX = 0.0f;
    private float _yOffset;
    private float _xOffset;

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _composer = _virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        _yOffset = _composer.m_TrackedObjectOffset.y;
        _xOffset = _composer.m_TrackedObjectOffset.x;
    }

    private void Update()
    {
        TrackVertical();
    }

    private void TrackVertical()
    {
        _mouseY += Input.GetAxis("Mouse Y");
        _mouseY = Mathf.Clamp(_mouseY, -maxTilt, maxTilt);
        
        _mouseX += Input.GetAxis("Mouse X");
        _mouseX = Mathf.Clamp(_mouseX, -maxPan, maxPan);
        
        _composer.m_TrackedObjectOffset.y = _yOffset + _mouseY;
        _composer.m_TrackedObjectOffset.x = _xOffset + _mouseX;
    }
    
    private bool IsGravityNegative()
    {
        Vector3 gravNormed = Physics.gravity.normalized;
        return (gravNormed.x + gravNormed.y + gravNormed.z) < 0.0f;
    }
}
