using TMPro;
using UnityEngine;

public class HUDLogic_EndlessRunnerPOE : MonoBehaviour
{
    [Header("HUD Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI bossStatusText;
    public TextMeshProUGUI doublePointsText;
    public TextMeshProUGUI invulnerabilityText;

    [Header("Game Data")]
    public int score = 0;
    public int lives = 3;

    void Start()
    {
        UpdateLives(lives);
        UpdateScore();
        UpdateShield(false);
        UpdateBossStatus("Boss: Waiting");
        UpdateDoublePoints(false, 0f);
        UpdateInvulnerability(false, 0f);
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

    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateScore();
    }

    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void UpdateLives(int currentLives)
    {
        lives = currentLives;

        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    public void UpdateShield(bool shieldActive)
    {
        if (shieldText != null)
        {
            shieldText.text = shieldActive ? "Shield: ON" : "Shield: OFF";
        }
    }

    public void UpdateBossStatus(string status)
    {
        if (bossStatusText != null)
        {
            bossStatusText.text = status;
        }
    }

    public void UpdateDoublePoints(bool active, float timeRemaining)
    {
        if (doublePointsText == null)
        {
            return;
        }

        if (active)
        {
            doublePointsText.text = "Double Points: " + timeRemaining.ToString("F1") + "s";
        }
        else
        {
            doublePointsText.text = "Double Points: OFF";
        }
    }

    public void UpdateInvulnerability(bool active, float timeRemaining)
    {
        if (invulnerabilityText == null)
        {
            return;
        }

        if (active)
        {
            invulnerabilityText.text = "Invulnerable: " + timeRemaining.ToString("F1") + "s";
        }
        else
        {
            invulnerabilityText.text = "Invulnerable: OFF";
        }
    }

    public void ShowGameOver()
    {
        Debug.Log("Game Over");
    }

    void TimerProgress()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Time.timeSinceLevelLoad.ToString("F2") + "s";
        }
    }
}