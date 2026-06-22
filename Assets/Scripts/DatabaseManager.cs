using SQLite4Unity3d;
using UnityEngine.SocialPlatforms.Impl;
public class HighScore
{
    [PrimaryKey]
    public int Id { get; set; }
    public int LevelReached { get; set; }
    public string PlayerName { get; set; }

    public int Score { get; set; }
    public System.DateTime DateAchieved { get; set; }



}
public class GameState
{
    [PrimaryKey]
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public int CurrentScore { get; set; }
    public int LevelReached { get; set; }
}