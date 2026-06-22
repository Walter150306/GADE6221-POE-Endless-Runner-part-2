using UnityEngine;

public class RunProgressManager : MonoBehaviour
{
    private static RunProgressManager instance;

    public static RunProgressManager Instance
    {
        get
        {
            if (instance == null)
            {
                CreateInstance();
            }

            return instance;
        }
    }

    [Header("Run State")]
    public bool runActive = false;
    public int currentLives = 3;
    public int currentScore = 0;

    [Header("Level Progression")]
    public bool completedLevel1 = false;
    public bool completedLevel2 = false;
    public bool loopingUnlocked = false;
    public int levelsBeaten = 0;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (instance == null)
        {
            CreateInstance();
        }
    }

    private static void CreateInstance()
    {
        GameObject managerObject = new GameObject("RunProgressManager");
        managerObject.AddComponent<RunProgressManager>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartNewRun()
    {
        runActive = true;

        currentLives = 3;
        currentScore = 0;

        completedLevel1 = false;
        completedLevel2 = false;
        loopingUnlocked = false;
        levelsBeaten = 0;

        Debug.Log("RunProgressManager: New run started.");
    }

    public void EnsureRunStarted()
    {
        if (!runActive)
        {
            StartNewRun();
        }
    }

    public void SetLives(int lives)
    {
        currentLives = lives;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    public void EndRun()
    {
        runActive = false;
    }

    public string RegisterLevelCompleteAndGetNextScene(string completedSceneName)
    {
        levelsBeaten++;

        if (completedSceneName == "Level 1")
        {
            completedLevel1 = true;
        }

        if (completedSceneName == "Level 2")
        {
            completedLevel2 = true;
        }

        // First required progression:
        // Player must complete Level 1 before Level 2.
        if (completedSceneName == "Level 1" && !completedLevel2)
        {
            Debug.Log("RunProgressManager: First Level 1 complete. Loading Level 2.");
            return "Level 2";
        }

        // If Level 2 has just been completed, looping starts by going back to Level 1.
        if (completedSceneName == "Level 2")
        {
            loopingUnlocked = completedLevel1 && completedLevel2;

            Debug.Log("RunProgressManager: Level 2 complete. Loading Level 1.");
            return "Level 1";
        }

        // If Level 1 is completed after looping has started, go to Level 2.
        if (completedSceneName == "Level 1" && completedLevel2)
        {
            loopingUnlocked = true;

            Debug.Log("RunProgressManager: Looping active. Loading Level 2.");
            return "Level 2";
        }

        // Safety fallback.
        Debug.LogWarning("RunProgressManager: Unexpected progression state. Loading Level 1.");
        return "Level 1";
    }
}