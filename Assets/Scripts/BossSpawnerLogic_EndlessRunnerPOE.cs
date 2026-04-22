using UnityEngine;

public class BossSpawnerLogic_EndlessRunnerPOE : MonoBehaviour
{
    [Header("References")]
    public GameObject bossPrefab;
    public Transform player;
    public BossEnvironmentLogic bossEnvironment;
    public FloorSpawnerLogic floorSpawner;

    [Header("Spawn Position")]
    public float spawnX = -8f;
    public float riseStartY = -4f;
    public float finalBossY = 2f;
    public float spawnDistanceAhead = -15f;

    [Header("Fight Settings")]
    public float fightDuration = 30f;

    private bool bossSpawned = false;
    private bool bossPhaseFinished = false;
    private GameObject currentBoss;

    public void TriggerBossRise()
    {
        Debug.Log("BossSpawner: TriggerBossRise called.");

        if (bossSpawned || bossPhaseFinished)
        {
            Debug.Log("BossSpawner: Boss already active or phase already finished.");
            return;
        }

        SpawnBoss();
    }

    void SpawnBoss()
    {
        if (bossPrefab == null)
        {
            Debug.LogError("BossSpawner: bossPrefab is not assigned.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("BossSpawner: player is not assigned.");
            return;
        }

        Vector3 spawnPosition = new Vector3(
            spawnX,
            riseStartY,
            player.position.z + spawnDistanceAhead
        );

        Debug.Log("BossSpawner: Spawning boss at " + spawnPosition);

        currentBoss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);

        LevelBossOne_Logic_EndlessRunnerPOE bossLogic = currentBoss.GetComponent<LevelBossOne_Logic_EndlessRunnerPOE>();

        if (bossLogic != null)
        {
            bossLogic.player = player;
            bossLogic.startY = riseStartY;
            bossLogic.finalY = finalBossY;
            bossLogic.BeginRise();
        }
        else
        {
            Debug.LogError("BossSpawner: Spawned boss is missing LevelBossOne_Logic_EndlessRunnerPOE.");
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
        Invoke(nameof(EndBossFight), fightDuration);
    }

    void EndBossFight()
    {
        Debug.Log("BossSpawner: Ending boss fight.");

        if (currentBoss != null)
        {
            Destroy(currentBoss);
        }

        if (bossEnvironment != null)
        {
            bossEnvironment.ExitBossPhase();
        }

        if (player != null)
        {
            PlayerCubeLogic_3dIntroDemo playerLogic = player.GetComponent<PlayerCubeLogic_3dIntroDemo>();

            if (playerLogic != null)
            {
                playerLogic.ExitBossWaterPhase();
            }
        }

        if (floorSpawner != null)
        {
            floorSpawner.ForceBossExitPhase();
        }

        bossSpawned = false;
        bossPhaseFinished = true;
    }
}