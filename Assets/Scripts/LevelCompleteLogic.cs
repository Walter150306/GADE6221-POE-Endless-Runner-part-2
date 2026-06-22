using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteLogic : MonoBehaviour
{
    [Header("Timing")]
    public float stopSpawningTime = 110f;
    public float levelCompleteTime = 120f;

    [Header("Level Display")]
    public string currentLevelDisplayName = "Level 1";

    [Header("Auto Level Transition")]
    public bool autoLoadNextLevel = true;
    public float autoLoadDelay = 1.5f;

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
    private string resolvedNextSceneName = "Level 2";

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

        GameEventManager.Instance.LevelBeaten();

        string completedSceneName = SceneManager.GetActiveScene().name;
        resolvedNextSceneName =
            RunProgressManager.Instance.RegisterLevelCompleteAndGetNextScene(completedSceneName);

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
            levelCompleteText.text =
                currentLevelDisplayName +
                " Completed!\nLoading " +
                resolvedNextSceneName +
                "...";
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Current Score: " + RunProgressManager.Instance.currentScore;
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Debug.Log(
            "LevelCompleteLogic: " +
            currentLevelDisplayName +
            " complete. Loading " +
            resolvedNextSceneName
        );

        if (autoLoadNextLevel)
        {
            StartCoroutine(AutoLoadNextLevelAfterDelay());
        }
    }

    IEnumerator AutoLoadNextLevelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(autoLoadDelay);
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(resolvedNextSceneName);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        RunProgressManager.Instance.StartNewRun();
        GameEventManager.Instance.ResetRunMetrics();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;

        RunProgressManager.Instance.EndRun();

        SceneManager.LoadScene("Main menu");
    }
}