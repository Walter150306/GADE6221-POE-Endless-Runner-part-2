using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

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

        lives = 3;
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
            return;
        }

        lives--;

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

        if (hudLogic != null)
        {
            hudLogic.AddScore(finalAmount);
        }
    }

    void GameOver()
    {
        isGameOver = true;

        int finalScore = 0;

        if (hudLogic != null)
        {
            finalScore = hudLogic.score;
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

        Time.timeScale = 0f;

        Debug.Log("Game Over screen shown.");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main menu");
    }

    
}