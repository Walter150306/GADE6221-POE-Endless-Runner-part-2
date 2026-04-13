using TMPro;
using UnityEngine;

public class HUDLogic_EndlessRunnerPOE : MonoBehaviour
{
    [Header("HUD Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI shieldText;

    [Header("Game Data")]
    public int score = 0;
    public int lives = 3;

    void Start()
    {
        UpdateLives(lives);
        UpdateScore();
        UpdateShield(false);
    }

    void Update()
    {
        TimerProgress();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        lives = currentLives;
        livesText.text = "Lives: " + currentLives;
    }

    public void UpdateShield(bool shieldActive)
    {
        if (shieldText != null)
        {
            if (shieldActive)
            {
                shieldText.text = "Shield: ON";
            }
            else
            {
                shieldText.text = "Shield: OFF";
            }
        }
    }

    public void ShowGameOver()
    {
        Debug.Log("Game Over");
    }

    void TimerProgress()
    {
        timerText.text = "Time: " + Time.timeSinceLevelLoad.ToString("F2") + "s";
    }
}