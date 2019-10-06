using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public int countdownSeconds = 30;
    
    private Text _countdownText;
    private int _currentTicks = 0;

    private void Start()
    {
        _countdownText = GetComponent<Text>();
        _countdownText.enabled = false;
    }

    public void OnTick()
    {
        string text = (countdownSeconds - _currentTicks).ToString();
        _countdownText.enabled = true;
        _countdownText.text = text;
        _currentTicks++;
    }

    public void ResetCounter()
    {
        _countdownText.enabled = false;
        _countdownText.text = "";
        _currentTicks = 0;
    }

    public void Dead()
    {
        _countdownText.enabled = true;
        _countdownText.text = "Your friend is gone";
        _currentTicks = 0;
    }
}
