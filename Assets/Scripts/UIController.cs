using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController instance;

    public static UIController GetInstance()
    {
        if (instance)
        {
            return instance;
        }

        instance = FindObjectOfType<UIController>();
        return instance;
    }
    
    public int countdownSeconds = 30;
    public Text countdownText;
    
    private int _currentTicks;

    private void Start()
    {
        countdownText.enabled = false;
    }

    public void ResetCountdown()
    {
        countdownText.enabled = false;
        countdownText.text = "";
        _currentTicks = 0;
    }
    
    public void CountdownTick()
    {
        if (_currentTicks == countdownSeconds)
        {
            ResetCountdown();
            ShowGameOver();
        }
        else
        {
            float progress = _currentTicks / (float)countdownSeconds;
            string text = (countdownSeconds - _currentTicks).ToString();
            countdownText.enabled = true;
            countdownText.color = Color.Lerp(Color.yellow, Color.red, progress);
            countdownText.text = text;
            _currentTicks++;
        }
    }

    private void ShowGameOver()
    {
        
    }
}
