using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor2 : MonoBehaviour
{
    public static int pressurePlateCounter = 0;
    private bool isDoorLocked = true;
    public float openingSpeed = 5f;
    public float threshold = 134;
    public WindowBlocker window1;
    public WindowBlocker window2;

    private AudioSource _audioSource;
    private bool _isPuzzleSolved = false;

    // Start is called before the first frame update
    void Start()
    {
        pressurePlateCounter = 0;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pressurePlateCounter == 3)
        {
            _isPuzzleSolved = true;
        }
        
        if (_isPuzzleSolved &&  transform.position.y <= threshold)
        {
            transform.position += new Vector3(0, openingSpeed  * Time.deltaTime, 0);
            _audioSource.Play();
            window1.enabled = false;
            window2.enabled = false;
            MusicController.GetInstance().variation = MusicController.Variation.D_NoLowpass;
        }
    }

    public void IncrementPlateCounter()
    {
        pressurePlateCounter++;
    }

    public void DecrementPlateCounter()
    {
        pressurePlateCounter--;
    }
}
