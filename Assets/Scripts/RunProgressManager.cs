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

        // First required progression: Level 1 must lead to Level 2.
        if (completedLevel1 && !completedLevel2)
        {
            Debug.Log("RunProgressManager: Level 1 complete. Loading Level 2.");
            return "Level 2";
        }

        // Safety: if somehow Level 2 was played first, force Level 1 next.
        if (!completedLevel1)
        {
            Debug.Log("RunProgressManager: Level 1 was not completed yet. Loading Level 1.");
            return "Level 1";
        }

        // After Level 1 and Level 2 are both complete, looping begins.
        loopingUnlocked = true;

        string nextScene = Random.value < 0.5f ? "Level 1" : "Level 2";

        Debug.Log("RunProgressManager: Looping unlocked. Next scene = " + nextScene);

        return nextScene;
    }
}