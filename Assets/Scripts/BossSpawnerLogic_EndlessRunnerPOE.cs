using UnityEngine;

public class BossSpawnerLogic_EndlessRunnerPOE : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform player;
    public BossEnvironmentLogic bossEnvironment;

    private bool bossSpawned = false;

    [Header("Spawn Settings")]
    public float spawnDistanceAhead = 20f;
    public float spawnHeight = 3f;

    void Update()
    {
        if (!bossSpawned && Input.GetKeyDown(KeyCode.B))
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null || player == null)
        {
            Debug.LogWarning("Boss prefab or player not assigned.");
            return;
        }

        FloorSpawnerLogic floorSpawner = FindObjectOfType<FloorSpawnerLogic>();

        if (floorSpawner != null)
        {
            floorSpawner.StartBossTunnelPhase();
        }

        Vector3 spawnPosition = new Vector3(
            player.position.x,
            spawnHeight,
            player.position.z + spawnDistanceAhead
        );

        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);

        LevelBossOne_Logic_EndlessRunnerPOE bossLogic = boss.GetComponent<LevelBossOne_Logic_EndlessRunnerPOE>();

        if (bossLogic != null)
        {
            bossLogic.player = player;
        }

        if (bossEnvironment != null)
        {
            bossEnvironment.EnterBossPhase();
        }

        PlayerCubeLogic_3dIntroDemo playerLogic = player.GetComponent<PlayerCubeLogic_3dIntroDemo>();

        if (playerLogic != null)
        {
            playerLogic.EnterBossWaterPhase();
        }

        bossSpawned = true;
    }
}