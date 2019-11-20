using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

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
    public Text gameOverText;
    public Image gameOverPanel;
    public Button restartButton;
    
    private int _currentTicks;

    private void Start()
    {
        countdownText.enabled = false;
        gameOverText.enabled = false;
        gameOverPanel.enabled = false;
        restartButton.enabled = false;
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

    public void ShowGameOver()
    {
        
    }

    private IEnumerator GameOverAnimation()
    {
        float progress = 0;

        gameOverText.enabled = true;
        gameOverPanel.enabled = true;
        
        while (progress < 1.0)
        {
            gameOverPanel.color = Color.Lerp(Color.clear, Color.black, progress);
            gameOverText.color = Color.Lerp(Color.clear, Color.white, progress);
            progress += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        progress = 0f;

        while (progress < 1.0)
        {
            progress += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        
        restartButton.enabled = true;
    }
}
