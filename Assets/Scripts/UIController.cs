using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public int hintTimeSeconds = 5;
    public Text countdownText;
    public Text gameOverText;
    public Image gameOverPanel;
    public Button restartButton;
    public Text hintsText;
    
    private int _currentTicks;
    private bool _shouldShowTetherHint = true;
    private bool _shouldShowRunHint = true;
    private bool _shouldShowValveHint = true;
    private bool _shouldShowLightHint = true;
    private bool _shouldShowJumpHint = true;

    private void Start()
    {
        Cursor.visible = false;
        countdownText.enabled = false;
        gameOverText.enabled = false;
        gameOverPanel.enabled = false;
        hintsText.enabled = false;
        restartButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Scenes/Splash");
        }
        
        if (Input.GetButton("Fire2"))
        {
            _shouldShowTetherHint = false;
            ToggleTetherHint(false);
        }
    }

    public void ShowLightHint()
    {
        if (_shouldShowLightHint)
        {
            hintsText.enabled = true;
            hintsText.text = "Step out of the light!";
            _shouldShowLightHint = false;
            StartCoroutine(HintTextTimer());
        }
    }

    public void ShowJumpHint()
    {
        if (_shouldShowJumpHint)
        {
            hintsText.enabled = true;
            hintsText.text = "Run towards the block to jump over.";
            _shouldShowJumpHint = false;
            StartCoroutine(HintTextTimer());
        }
    }

    public void ShowTetherHint()
    {
        if (_shouldShowTetherHint)
        {
            ToggleTetherHint(true);
            _shouldShowTetherHint = false;
            StartCoroutine(HintTextTimer());
        }
    }

    public void ShowRunHint()
    {
        if (_shouldShowRunHint)
        {
            hintsText.enabled = true;
            hintsText.text = "Hold shift to run.";
            _shouldShowRunHint = false;
            StartCoroutine(HintTextTimer());
        }
    }
    
    public void ShowValveHint()
    {
        if (_shouldShowValveHint)
        {
            hintsText.enabled = true;
            hintsText.text = "Left-click the mouse to use the valve.";
            _shouldShowValveHint = false;
            StartCoroutine(HintTextTimer());
        }
    }

    public void ShowEndingCredits()
    {
        StartCoroutine(GameEndAnimation());
    }

    public void ToggleTetherHint(bool isEnabled)
    {
        hintsText.enabled = isEnabled;
        hintsText.text = isEnabled ? "Right-click the mouse to toggle the tether." : "";
    }

    public bool ShouldShowTetherHint()
    {
        return _shouldShowTetherHint;
    }

    private IEnumerator HintTextTimer()
    {
        yield return new WaitForSeconds(hintTimeSeconds);
        hintsText.enabled = false;
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
        StartCoroutine(GameOverAnimation());
    }

    private IEnumerator GameEndAnimation()
    {
        float progress = 0;

        gameOverPanel.color = Color.clear;
        gameOverText.color = Color.clear;
        
        gameOverText.enabled = true;
        gameOverPanel.enabled = true;

        gameOverText.text = "To Be Continued...";
        
        while (progress <= 1.0)
        {
            gameOverPanel.color = Color.Lerp(Color.clear, Color.black, progress);
            gameOverText.color = Color.Lerp(Color.clear, Color.white, progress);
            progress += 0.01f;
            yield return new WaitForEndOfFrame();
        }
        
        restartButton.gameObject.SetActive(true);
        
        while (!Input.GetButton("Fire1"))
        {
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("Scenes/Splash");
    }

    private IEnumerator GameOverAnimation()
    {
        float progress = 0;

        gameOverPanel.color = Color.clear;
        gameOverText.color = Color.clear;
        
        gameOverText.enabled = true;
        gameOverPanel.enabled = true;
        
        while (progress <= 1.0)
        {
            gameOverPanel.color = Color.Lerp(Color.clear, Color.black, progress);
            gameOverText.color = Color.Lerp(Color.clear, Color.white, progress);
            progress += 0.01f;
            yield return new WaitForEndOfFrame();
        }
        
        restartButton.gameObject.SetActive(true);

        while (!Input.GetButton("Fire1"))
        {
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("Scenes/Levels");
    }
}
