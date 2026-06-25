using UnityEngine;
using TMPro;

public class TimerGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject scoreboardPanel;
    
    private float countdownTime = 60f;
    private float timeRemaining;
    private int score = 0;
    private bool isGameActive = false;

    private void Start()
    {
        timeRemaining = countdownTime;
        ShowStartScreen();
    }

    private void Update()
    {
        if (isGameActive)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();

            if (timeRemaining <= 0)
            {
                EndGame();
            }
        }
    }

    private void ShowStartScreen()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        scoreboardPanel.SetActive(false);
        isGameActive = false;
    }

    public void StartGame()
    {
        timeRemaining = countdownTime;
        score = 0;
        isGameActive = true;
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        scoreboardPanel.SetActive(false);
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddScore(int points)
    {
        if (isGameActive)
        {
            score += points;
            scoreText.text = "Score: " + score;
        }
    }

    private void EndGame()
    {
        isGameActive = false;
        gamePanel.SetActive(false);
        scoreboardPanel.SetActive(true);
        DisplayScoreboard();
    }

    private void DisplayScoreboard()
    {
        TextMeshProUGUI scoreboardText = scoreboardPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (scoreboardText != null)
        {
            scoreboardText.text = "GAME OVER\n\nFinal Score: " + score;
        }
    }

    public void RestartGame()
    {
        ShowStartScreen();
    }
}
