using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerLogic_EndlessRunner : MonoBehaviour
{
    public static GameManagerLogic_EndlessRunner instance;

    [Header("Game Settings")]
    public int lives = 3;
    private bool isGameOver = false;

    [Header("Shield Settings")]
    public bool hasShield = false;

    [Header("Double Points Settings")]
    public bool doublePointsActive = false;
    public float doublePointsDuration = 15f;
    private float doublePointsTimer = 0f;

    [Header("Invulnerability Settings")]
    public bool invulnerabilityActive = false;
    public float invulnerabilityDuration = 7f;
    private float invulnerabilityTimer = 0f;

    [Header("References")]
    public HUDLogic_EndlessRunnerPOE hudLogic;
    public ObstacleSpawnerLogic obstacleSpawner;
    public FloorSpawnerLogic floorSpawner;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        RunProgressManager.Instance.EnsureRunStarted();

        lives = RunProgressManager.Instance.currentLives;

        hasShield = false;
        isGameOver = false;

        doublePointsActive = false;
        doublePointsTimer = 0f;

        invulnerabilityActive = false;
        invulnerabilityTimer = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (hudLogic != null)
        {
            hudLogic.SetScore(RunProgressManager.Instance.currentScore);
            hudLogic.UpdateLives(lives);
            hudLogic.UpdateShield(false);
            hudLogic.UpdateDoublePoints(false, 0f);
            hudLogic.UpdateInvulnerability(false, 0f);
        }
    }

    void Update()
    {
        HandleDoublePointsTimer();
        HandleInvulnerabilityTimer();
    }

    void HandleDoublePointsTimer()
    {
        if (!doublePointsActive)
        {
            return;
        }

        doublePointsTimer -= Time.deltaTime;

        if (doublePointsTimer <= 0f)
        {
            doublePointsTimer = 0f;
            doublePointsActive = false;
        }

        if (hudLogic != null)
        {
            hudLogic.UpdateDoublePoints(doublePointsActive, doublePointsTimer);
        }
    }

    void HandleInvulnerabilityTimer()
    {
        if (!invulnerabilityActive)
        {
            return;
        }

        invulnerabilityTimer -= Time.deltaTime;

        if (invulnerabilityTimer <= 0f)
        {
            invulnerabilityTimer = 0f;
            invulnerabilityActive = false;
        }

        if (hudLogic != null)
        {
            hudLogic.UpdateInvulnerability(invulnerabilityActive, invulnerabilityTimer);
        }
    }

    public void ActivateShield()
    {
        if (isGameOver)
        {
            return;
        }

        hasShield = true;

        if (hudLogic != null)
        {
            hudLogic.UpdateShield(true);
        }

        Debug.Log("Shield activated");
    }

    public void ActivateDoublePoints()
    {
        if (isGameOver)
        {
            return;
        }

        doublePointsActive = true;
        doublePointsTimer = doublePointsDuration;

        if (hudLogic != null)
        {
            hudLogic.UpdateDoublePoints(true, doublePointsTimer);
        }

        Debug.Log("Double points activated");
    }

    public void ActivateInvulnerability()
    {
        if (isGameOver)
        {
            return;
        }

        invulnerabilityActive = true;
        invulnerabilityTimer = invulnerabilityDuration;

        if (hudLogic != null)
        {
            hudLogic.UpdateInvulnerability(true, invulnerabilityTimer);
        }

        Debug.Log("Invulnerability activated");
    }

    public void PlayerGetsHit()
    {
        if (isGameOver)
        {
            return;
        }

        if (invulnerabilityActive)
        {
            Debug.Log("Invulnerability blocked the hit");
            return;
        }

        if (hasShield)
        {
            hasShield = false;

            if (hudLogic != null)
            {
                hudLogic.UpdateShield(false);
            }

            Debug.Log("Shield blocked the hit");

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayShieldBlock();
            }
            return;
        }

        lives--;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayHit();
        }
        RunProgressManager.Instance.SetLives(lives);

        if (hudLogic != null)
        {
            hudLogic.UpdateLives(lives);
        }

        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver)
        {
            return;
        }

        int finalAmount = amount;

        if (doublePointsActive)
        {
            finalAmount *= 2;
        }

        RunProgressManager.Instance.AddScore(finalAmount);

        if (hudLogic != null)
        {
            hudLogic.SetScore(RunProgressManager.Instance.currentScore);
        }
    }

    void GameOver()
    {
        isGameOver = true;

        int finalScore = RunProgressManager.Instance.currentScore;

        if (hudLogic != null)
        {
            hudLogic.ShowGameOver();
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + finalScore;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (DatabaseManager.Instance != null)
        {
            DatabaseManager.Instance.UpdateGameOverMetricsText();
        }

        RunProgressManager.Instance.EndRun();

        Time.timeScale = 0f;

        Debug.Log("Game Over screen shown.");

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameOver();
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        if (RunProgressManager.Instance != null)
        {
            RunProgressManager.Instance.StartNewRun();
        }

        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.ResetRunMetrics();
        }

        SceneManager.LoadScene("Level 1");
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;

        if (RunProgressManager.Instance != null)
        {
            RunProgressManager.Instance.EndRun();
        }

        SceneManager.LoadScene("Main menu");
    }
}