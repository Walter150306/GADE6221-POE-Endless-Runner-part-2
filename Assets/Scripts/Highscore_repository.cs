using System.IO;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;
using System.Linq;

public class HighscoreRepository 
{
    private readonly SQLiteConnection _db;
    
    public HighscoreRepository()
    {
        var dbPath = Path.Combine(Application.persistentDataPath, "game.db");
        Debug.Log($"[DB] Using database at: {dbPath}");

        _db = new SQLiteConnection(dbPath,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // Create both tables
        _db.CreateTable<GameState>();
        _db.CreateTable<HighScore>();

        // Seed/reset GameState each startup
        if (_db.Find<GameState>(1) == null)
        {
            _db.Insert(new GameState { Id = 1, PlayerName = "", CurrentScore = 0, LevelReached = 1 });
            Debug.Log("[DB] Seeded initial GameState");
        }
        else
        {
            ResetGameState();
        }
    }

    // --- GameState methods ---
    public GameState LoadGameState()
    {
        return _db.Get<GameState>(1);
    }

    public void UpdateGameState(int score, int level, string playerName = null)
    {
        var state = _db.Get<GameState>(1);
        state.CurrentScore = score;
        state.LevelReached = level;
        if (!string.IsNullOrEmpty(playerName))
            state.PlayerName = playerName;
        _db.Update(state);
    }

    public void ResetGameState()
    {
        var state = _db.Get<GameState>(1);
        state.CurrentScore = 0;
        state.LevelReached = 1;
        _db.Update(state);
    }

    // --- HighScore methods ---
    public void SaveHighScore(string playerName, int score, int levelReached)
    {
        var hs = new HighScore
        {
            PlayerName = playerName,
            Score = score,
            LevelReached = levelReached,
            DateAchieved = System.DateTime.Now
        };
        _db.Insert(hs);
    }

    public List<HighScore> GetTopScores(int limit = 10)
    {
        return _db.Table<HighScore>()
                  .OrderByDescending(hs => hs.Score)
                  .Take(limit)
                  .ToList();
    }

    public void ClearScores()
    {
        _db.DeleteAll<HighScore>();
    }
    public void SaveHighScoreFromGameState()
{
    var state = LoadGameState();
    if (string.IsNullOrEmpty(state.PlayerName))
    {
        Debug.LogWarning("[HighscoreRepository] Player name not set, using 'Anonymous'.");
        state.PlayerName = "Anonymous";
    }

    SaveHighScore(state.PlayerName, state.CurrentScore, state.LevelReached);
    Debug.Log($"[HighscoreRepository] High score saved for {state.PlayerName} with {state.CurrentScore} points at level {state.LevelReached}.");
}

    public void Close()
    {
        _db?.Close();
    }
}

// --- Data Models ---


/*public class HighScore
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public int Score { get; set; }
    public int LevelReached { get; set; }
    public System.DateTime DateAchieved { get; set; }
}*/


// Data model for highscores
/*public class HighScore
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string PlayerName { get; set; }
    public int Score { get; set; }
    public int LevelReached { get; set; }
    public System.DateTime DateAchieved { get; set; }
}
*/