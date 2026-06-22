using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScore
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string PlayerName { get; set; }

    public int Score { get; set; }
    public int LevelReached { get; set; }
    public int LevelsBeaten { get; set; }

    public int ObstaclesPassed { get; set; }

    public int ShieldPickupsActivated { get; set; }
    public int DoublePointsPickupsActivated { get; set; }
    public int InvulnerabilityPickupsActivated { get; set; }

    public int Boss1Spawned { get; set; }
    public int Boss2Spawned { get; set; }
    public int Boss1Beaten { get; set; }
    public int Boss2Beaten { get; set; }

    public DateTime DateAchieved { get; set; }
}

public class GameState
{
    [PrimaryKey]
    public int Id { get; set; }

    public string PlayerName { get; set; }
    public int CurrentScore { get; set; }
    public int LevelReached { get; set; }
}

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    [Header("Optional UI")]
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI leaderboardText;
    public TextMeshProUGUI gameOverMetricsText;

    private HighscoreRepository repository;

    void Awake()
    {
        Instance = this;
        CreateRepository();
    }

    void Start()
    {
        UpdateGameOverMetricsText();
    }

    void CreateRepository()
    {
        try
        {
            repository = new HighscoreRepository();
            Debug.Log("[DatabaseManager] Repository ready.");
        }
        catch (Exception exception)
        {
            Debug.LogError("[DatabaseManager] Failed to create repository: " + exception);
            repository = null;
        }
    }

    string GetPlayerName()
    {
        if (playerNameInput != null && !string.IsNullOrWhiteSpace(playerNameInput.text))
        {
            return playerNameInput.text;
        }

        return "Player";
    }

    public void SaveCurrentRunScoreButton()
    {
        if (repository == null)
        {
            CreateRepository();
        }

        if (repository == null)
        {
            Debug.LogError("[DatabaseManager] Cannot save score because repository is missing.");
            return;
        }

        string playerName = GetPlayerName();

        repository.SaveHighScoreFromCurrentRun(playerName);

        Debug.Log("[DatabaseManager] Saved current run score for " + playerName);

        RefreshLeaderboardText();
    }

    public void RefreshLeaderboardText()
    {
        if (repository == null)
        {
            CreateRepository();
        }

        if (repository == null)
        {
            return;
        }

        List<HighScore> scores = repository.GetTopScores(10);

        if (leaderboardText == null)
        {
            foreach (HighScore score in scores)
            {
                Debug.Log(
                    score.PlayerName +
                    " | Score: " + score.Score +
                    " | Levels Beaten: " + score.LevelsBeaten +
                    " | Obstacles: " + score.ObstaclesPassed
                );
            }

            return;
        }

        if (scores.Count == 0)
        {
            leaderboardText.text = "No scores saved yet.";
            return;
        }

        string output = "High Scores\n\n";

        for (int i = 0; i < scores.Count; i++)
        {
            HighScore score = scores[i];

            output +=
                (i + 1) + ". " +
                score.PlayerName +
                " - Score: " + score.Score +
                " | Levels: " + score.LevelsBeaten +
                " | Obstacles: " + score.ObstaclesPassed +
                "\n";
        }

        leaderboardText.text = output;
    }

    public void UpdateGameOverMetricsText()
    {
        if (gameOverMetricsText == null)
        {
            return;
        }

        int finalScore = 0;
        int levelsBeaten = 0;
        int obstaclesPassed = 0;
        int pickupsCollected = 0;
        int bossesBeaten = 0;

        if (RunProgressManager.Instance != null)
        {
            finalScore = RunProgressManager.Instance.currentScore;
            levelsBeaten = RunProgressManager.Instance.levelsBeaten;
        }

        if (GameEventManager.Instance != null)
        {
            obstaclesPassed = GameEventManager.Instance.obstaclesPassed;

            pickupsCollected =
                GameEventManager.Instance.shieldPickupsActivated +
                GameEventManager.Instance.doublePointsPickupsActivated +
                GameEventManager.Instance.invulnerabilityPickupsActivated;

            bossesBeaten =
                GameEventManager.Instance.boss1Beaten +
                GameEventManager.Instance.boss2Beaten;
        }

        gameOverMetricsText.text =
            "Final Score: " + finalScore +
            "\nLevels Beaten: " + levelsBeaten +
            "\nObstacles Passed: " + obstaclesPassed +
            "\nPickups Collected: " + pickupsCollected +
            "\nBosses Beaten: " + bossesBeaten;
    }

    public void ClearSavedScoresButton()
    {
        if (repository == null)
        {
            CreateRepository();
        }

        if (repository == null)
        {
            Debug.LogError("[DatabaseManager] Cannot clear scores because repository is missing.");
            return;
        }

        repository.ClearScores();

        Debug.Log("[DatabaseManager] Cleared all saved high scores.");

        RefreshLeaderboardText();
    }

    void OnDestroy()
    {
        if (repository != null)
        {
            repository.Close();
        }
    }
}