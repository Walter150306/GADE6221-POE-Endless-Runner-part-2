using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;

public class HighscoreRepository
{
    private readonly SQLiteConnection _db;

    public HighscoreRepository()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "game.db");

        Debug.Log("[DB] Using database at: " + dbPath);

        _db = new SQLiteConnection(
            dbPath,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create
        );

        _db.CreateTable<GameState>();
        _db.CreateTable<HighScore>();

        if (_db.Find<GameState>(1) == null)
        {
            _db.Insert(new GameState
            {
                Id = 1,
                PlayerName = "Player",
                CurrentScore = 0,
                LevelReached = 1
            });

            Debug.Log("[DB] Seeded initial GameState.");
        }
    }

    public GameState LoadGameState()
    {
        return _db.Find<GameState>(1);
    }

    public void UpdateGameState(int score, int levelReached, string playerName = "Player")
    {
        GameState state = _db.Find<GameState>(1);

        if (state == null)
        {
            state = new GameState
            {
                Id = 1,
                PlayerName = playerName,
                CurrentScore = score,
                LevelReached = levelReached
            };

            _db.Insert(state);
            return;
        }

        state.CurrentScore = score;
        state.LevelReached = levelReached;

        if (!string.IsNullOrEmpty(playerName))
        {
            state.PlayerName = playerName;
        }

        _db.Update(state);
    }

    public void ResetGameState()
    {
        GameState state = _db.Find<GameState>(1);

        if (state == null)
        {
            _db.Insert(new GameState
            {
                Id = 1,
                PlayerName = "Player",
                CurrentScore = 0,
                LevelReached = 1
            });

            return;
        }

        state.CurrentScore = 0;
        state.LevelReached = 1;
        _db.Update(state);
    }

    public void SaveHighScore(
        string playerName,
        int score,
        int levelReached,
        int levelsBeaten,
        int obstaclesPassed,
        int shieldPickupsActivated,
        int doublePointsPickupsActivated,
        int invulnerabilityPickupsActivated,
        int boss1Spawned,
        int boss2Spawned,
        int boss1Beaten,
        int boss2Beaten
    )
    {
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
        }

        HighScore highScore = new HighScore
        {
            PlayerName = playerName,

            Score = score,
            LevelReached = levelReached,
            LevelsBeaten = levelsBeaten,

            ObstaclesPassed = obstaclesPassed,

            ShieldPickupsActivated = shieldPickupsActivated,
            DoublePointsPickupsActivated = doublePointsPickupsActivated,
            InvulnerabilityPickupsActivated = invulnerabilityPickupsActivated,

            Boss1Spawned = boss1Spawned,
            Boss2Spawned = boss2Spawned,
            Boss1Beaten = boss1Beaten,
            Boss2Beaten = boss2Beaten,

            DateAchieved = System.DateTime.Now
        };

        _db.Insert(highScore);

        Debug.Log("[DB] Saved high score for " + playerName + " with score " + score);
    }

    // Backwards-compatible version, in case any button still calls the old method.
    public void SaveHighScore(string playerName, int score, int levelReached)
    {
        SaveHighScore(
            playerName,
            score,
            levelReached,
            levelReached,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        );
    }

    public void SaveHighScoreFromCurrentRun(string playerName = "Player")
    {
        int score = 0;
        int levelsBeaten = 0;
        int levelReached = 1;

        int obstaclesPassed = 0;

        int shieldPickups = 0;
        int doublePointsPickups = 0;
        int invulnerabilityPickups = 0;

        int boss1Spawned = 0;
        int boss2Spawned = 0;
        int boss1Beaten = 0;
        int boss2Beaten = 0;

        if (RunProgressManager.Instance != null)
        {
            score = RunProgressManager.Instance.currentScore;
            levelsBeaten = RunProgressManager.Instance.levelsBeaten;
            levelReached = Mathf.Max(1, levelsBeaten + 1);
        }

        if (GameEventManager.Instance != null)
        {
            obstaclesPassed = GameEventManager.Instance.obstaclesPassed;

            shieldPickups = GameEventManager.Instance.shieldPickupsActivated;
            doublePointsPickups = GameEventManager.Instance.doublePointsPickupsActivated;
            invulnerabilityPickups = GameEventManager.Instance.invulnerabilityPickupsActivated;

            boss1Spawned = GameEventManager.Instance.boss1Spawned;
            boss2Spawned = GameEventManager.Instance.boss2Spawned;
            boss1Beaten = GameEventManager.Instance.boss1Beaten;
            boss2Beaten = GameEventManager.Instance.boss2Beaten;
        }

        SaveHighScore(
            playerName,
            score,
            levelReached,
            levelsBeaten,
            obstaclesPassed,
            shieldPickups,
            doublePointsPickups,
            invulnerabilityPickups,
            boss1Spawned,
            boss2Spawned,
            boss1Beaten,
            boss2Beaten
        );
    }

    public List<HighScore> GetTopScores(int limit = 10)
    {
        return _db.Table<HighScore>()
            .OrderByDescending(highScore => highScore.Score)
            .Take(limit)
            .ToList();
    }

    public void ClearScores()
    {
        _db.DeleteAll<HighScore>();
    }

    public void Close()
    {
        _db?.Close();
    }
}