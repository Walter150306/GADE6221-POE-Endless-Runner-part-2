using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteLogic : MonoBehaviour
{
    [Header("Timing")]
    public float stopSpawningTime = 110f;
    public float levelCompleteTime = 120f;

    [Header("Level Looping")]
    public string currentLevelDisplayName = "Level 1";
    public string nextSceneName = "Level 2";

    [Header("References")]
    public ObstacleSpawnerLogic obstacleSpawner;
    public PickupSpawnerLogic pickupSpawner;
    public PlayerCubeLogic_3dIntroDemo playerLogic;
    public Rigidbody playerRigidbody;
    public HUDLogic_EndlessRunnerPOE hudLogic;

    [Header("Level Complete UI")]
    public GameObject levelCompletePanel;
    public TextMeshProUGUI levelCompleteText;
    public TextMeshProUGUI finalScoreText;

    private bool stoppedSpawning = false;
    private bool completedLevel = false;

    void Start()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        if (playerLogic != null && playerRigidbody == null)
        {
            playerRigidbody = playerLogic.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (completedLevel)
        {
            return;
        }

        float currentTime = Time.timeSinceLevelLoad;

        if (!stoppedSpawning && currentTime >= stopSpawningTime)
        {
            StopSpawningObjects();
        }

        if (currentTime >= levelCompleteTime)
        {
            CompleteLevel();
        }
    }

    void StopSpawningObjects()
    {
        stoppedSpawning = true;

        if (obstacleSpawner != null)
        {
            obstacleSpawner.enabled = false;
        }

        if (pickupSpawner != null)
        {
            pickupSpawner.enabled = false;
        }

        Debug.Log("LevelCompleteLogic: Obstacles and pickups stopped spawning.");
    }

    void CompleteLevel()
    {
        completedLevel = true;
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.LevelBeaten();
        }
        if (playerLogic != null)
        {
            playerLogic.enabled = false;
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.useGravity = false;
        }

        if (levelCompleteText != null)
        {
            levelCompleteText.text = currentLevelDisplayName + " Completed!";
        }

        if (finalScoreText != null && hudLogic != null)
        {
            finalScoreText.text = "Final Score: " + hudLogic.score;
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Debug.Log("LevelCompleteLogic: " + currentLevelDisplayName + " complete.");
    }

    public void LoadNextLevel()
    {
        if (string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.LogError("LevelCompleteLogic: Next Scene Name is empty.");
            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
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