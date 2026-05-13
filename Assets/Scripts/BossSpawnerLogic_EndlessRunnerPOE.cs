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
    public float spawnDistanceAhead = -20f;

    private bool bossSpawned = false;
    private bool bossPhaseFinished = false;
    private GameObject currentBoss;

    public void TriggerBossRise()
    {
        Debug.Log("BossSpawner: TriggerBossRise called.");

        if (bossSpawned)
        {
            Debug.Log("BossSpawner: Boss already spawned.");
            return;
        }

        if (bossPhaseFinished)
        {
            Debug.Log("BossSpawner: Boss phase already finished.");
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

        currentBoss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);

        LevelBossOne_Logic_EndlessRunnerPOE bossLogic =
            currentBoss.GetComponent<LevelBossOne_Logic_EndlessRunnerPOE>();

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

        PlayerCubeLogic_3dIntroDemo playerLogic =
            player.GetComponent<PlayerCubeLogic_3dIntroDemo>();

        if (playerLogic != null)
        {
            playerLogic.EnterBossWaterPhase();
        }

        bossSpawned = true;

        Debug.Log("BossSpawner: Boss started. Water mode ON.");
    }

    public void EndBossFight()
    {
        if (bossPhaseFinished)
        {
            Debug.Log("BossSpawner: Boss fight already finished.");
            return;
        }

        Debug.Log("BossSpawner: Ending boss fight. Water mode OFF.");

        if (currentBoss != null)
        {
            Destroy(currentBoss);
            currentBoss = null;
        }

        if (bossEnvironment != null)
        {
            bossEnvironment.ExitBossPhase();
        }

        if (player != null)
        {
            PlayerCubeLogic_3dIntroDemo playerLogic =
                player.GetComponent<PlayerCubeLogic_3dIntroDemo>();

            if (playerLogic != null)
            {
                playerLogic.ExitBossWaterPhase();
            }
        }

        bossSpawned = false;
        bossPhaseFinished = true;
    }
}