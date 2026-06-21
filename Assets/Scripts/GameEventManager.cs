using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    [Header("Run Metrics")]
    public int obstaclesPassed = 0;

    public int shieldPickupsActivated = 0;
    public int doublePointsPickupsActivated = 0;
    public int invulnerabilityPickupsActivated = 0;

    public int boss1Spawned = 0;
    public int boss2Spawned = 0;

    public int boss1Beaten = 0;
    public int boss2Beaten = 0;

    public int levelsBeaten = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetRunMetrics()
    {
        obstaclesPassed = 0;

        shieldPickupsActivated = 0;
        doublePointsPickupsActivated = 0;
        invulnerabilityPickupsActivated = 0;

        boss1Spawned = 0;
        boss2Spawned = 0;

        boss1Beaten = 0;
        boss2Beaten = 0;

        levelsBeaten = 0;

        Debug.Log("GameEventManager: Run metrics reset.");
    }

    public void ObstaclePassed()
    {
        obstaclesPassed++;
        Debug.Log("EVENT: Obstacle passed. Total = " + obstaclesPassed);
    }

    public void ShieldPickupActivated()
    {
        shieldPickupsActivated++;
        Debug.Log("EVENT: Shield pickup activated. Total = " + shieldPickupsActivated);
    }

    public void DoublePointsPickupActivated()
    {
        doublePointsPickupsActivated++;
        Debug.Log("EVENT: Double points pickup activated. Total = " + doublePointsPickupsActivated);
    }

    public void InvulnerabilityPickupActivated()
    {
        invulnerabilityPickupsActivated++;
        Debug.Log("EVENT: Invulnerability pickup activated. Total = " + invulnerabilityPickupsActivated);
    }

    public void BossSpawned(int bossNumber)
    {
        if (bossNumber == 1)
        {
            boss1Spawned++;
            Debug.Log("EVENT: Boss 1 spawned. Total = " + boss1Spawned);
        }
        else if (bossNumber == 2)
        {
            boss2Spawned++;
            Debug.Log("EVENT: Boss 2 spawned. Total = " + boss2Spawned);
        }
    }

    public void BossBeaten(int bossNumber)
    {
        if (bossNumber == 1)
        {
            boss1Beaten++;
            Debug.Log("EVENT: Boss 1 beaten. Total = " + boss1Beaten);
        }
        else if (bossNumber == 2)
        {
            boss2Beaten++;
            Debug.Log("EVENT: Boss 2 beaten. Total = " + boss2Beaten);
        }
    }

    public void LevelBeaten()
    {
        levelsBeaten++;
        Debug.Log("EVENT: Level beaten. Total = " + levelsBeaten);
    }
}