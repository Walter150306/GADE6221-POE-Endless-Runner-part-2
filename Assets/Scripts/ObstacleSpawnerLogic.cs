using UnityEngine;
using UnityEngine.Serialization;

public class ObstacleSpawnerLogic : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject barrelPrefab;
    public GameObject capsuleObstacle;
    public GameObject cylinderObstacle;

    [Header("References")]
    public Transform player;
    public BossSpawnerLogic_EndlessRunnerPOE bossSpawner;

    [Header("Spawn Timing")]
    public float spawnInterval = 2f;
    public float minSpawnInterval = 0.75f;
    public float difficultyRampEverySeconds = 10f;
    public float intervalDecreaseAmount = 0.25f;

    [Header("Spawn Position")]
    public float spawnY = 0.72f;
    public float spawnZ = -8f;

    [Header("Lane Positions - Player View")]
    [FormerlySerializedAs("farLeftLaneX")]
    public float farRightViewLaneX = -11f;

    [FormerlySerializedAs("leftLaneX")]
    public float rightViewLaneX = -8.7f;

    [FormerlySerializedAs("middleLaneX")]
    public float middleViewLaneX = -6.7f;

    [FormerlySerializedAs("rightLaneX")]
    public float leftViewLaneX = -4.8f;

    [FormerlySerializedAs("farRightLaneX")]
    public float farLeftViewLaneX = -2f;

    [Header("Spawn Fairness")]
    public int maxSameLaneRepeats = 1;

    [Header("Boss Phase")]
    public bool stopObstaclesDuringBoss = true;

    private float spawnTimer = 0f;
    private float difficultyTimer = 0f;

    private int lastLaneIndex = -1;
    private int sameLaneRepeatCount = 0;

    void Start()
    {
        if (bossSpawner == null)
        {
            bossSpawner = Object.FindFirstObjectByType<BossSpawnerLogic_EndlessRunnerPOE>();
        }
    }

    void Update()
    {
        if (stopObstaclesDuringBoss && bossSpawner != null && bossSpawner.BossPhaseInProgress)
        {
            return;
        }

        HandleDifficultyRamp();
        HandleObstacleSpawning();
    }

    void HandleDifficultyRamp()
    {
        difficultyTimer += Time.deltaTime;

        if (difficultyTimer >= difficultyRampEverySeconds)
        {
            spawnInterval -= intervalDecreaseAmount;
            spawnInterval = Mathf.Max(spawnInterval, minSpawnInterval);

            difficultyTimer = 0f;

            Debug.Log("ObstacleSpawner: New spawn interval = " + spawnInterval);
        }
    }

    void HandleObstacleSpawning()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnRandomObstacle();
            spawnTimer = 0f;
        }
    }

    void SpawnRandomObstacle()
    {
        GameObject prefabToSpawn = GetRandomObstaclePrefab();

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("ObstacleSpawner: Missing obstacle prefab.");
            return;
        }

        int laneIndex = GetFairRandomLaneIndex();
        float spawnX = GetLaneX(laneIndex);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

        GameObject obstacle = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        BarrelLogic barrelLogic = obstacle.GetComponent<BarrelLogic>();

        if (barrelLogic != null)
        {
            barrelLogic.player = player;
        }

        Debug.Log("ObstacleSpawner: Spawned " + obstacle.name + " in lane " + laneIndex + " at X " + spawnX);
    }

    GameObject GetRandomObstaclePrefab()
    {
        int choice = Random.Range(0, 3);

        if (choice == 0)
        {
            return barrelPrefab;
        }

        if (choice == 1)
        {
            return capsuleObstacle;
        }

        return cylinderObstacle;
    }

    int GetFairRandomLaneIndex()
    {
        int laneIndex = Random.Range(0, 5);

        if (laneIndex == lastLaneIndex)
        {
            sameLaneRepeatCount++;
        }
        else
        {
            sameLaneRepeatCount = 0;
        }

        if (sameLaneRepeatCount > maxSameLaneRepeats)
        {
            int newLaneIndex = laneIndex;

            while (newLaneIndex == lastLaneIndex)
            {
                newLaneIndex = Random.Range(0, 5);
            }

            laneIndex = newLaneIndex;
            sameLaneRepeatCount = 0;
        }

        lastLaneIndex = laneIndex;

        return laneIndex;
    }

    float GetLaneX(int laneIndex)
    {
        if (laneIndex == 0)
        {
            return farRightViewLaneX;
        }

        if (laneIndex == 1)
        {
            return rightViewLaneX;
        }

        if (laneIndex == 2)
        {
            return middleViewLaneX;
        }

        if (laneIndex == 3)
        {
            return leftViewLaneX;
        }

        return farLeftViewLaneX;
    }
}