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

    [Header("References")]
    public HUDLogic_EndlessRunnerPOE hudLogic;
    public ObstacleSpawnerLogic obstacleSpawner;
    public FloorSpawnerLogic floorSpawner;

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

        if (hudLogic != null)
        {
            hudLogic.UpdateLives(lives);
            hudLogic.UpdateShield(hasShield);
        }
    }

    public void ActivateShield()
    {
        hasShield = true;

        if (hudLogic != null)
        {
            hudLogic.UpdateShield(hasShield);
        }

        Debug.Log("Shield activated");
    }

    public void PlayerGetsHit()
    {
        if (isGameOver)
        {
            return;
        }

        if (hasShield)
        {
            hasShield = false;

            if (hudLogic != null)
            {
                hudLogic.UpdateShield(hasShield);
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

        if (hudLogic != null)
        {
            hudLogic.AddScore(amount);
        }
    }

    void GameOver()
    {
        isGameOver = true;

        if (hudLogic != null)
        {
            hudLogic.ShowGameOver();
        }

        SceneManager.LoadScene("Main menu");
    }
}