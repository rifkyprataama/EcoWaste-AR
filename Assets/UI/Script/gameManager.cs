using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;

    public float timer = 60f;

    public TMP_Text scoreText;
    public TMP_Text timerText;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        timerText.text =
            Mathf.Ceil(timer).ToString();

        if(timer <= 0)
        {
            GameOver();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;

        scoreText.text =
            "Score: " + score;
    }

    void GameOver()
    {
        Debug.Log("Game Over");
    }
}