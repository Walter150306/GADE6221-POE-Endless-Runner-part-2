using UnityEngine;

public class BossSpawnerLogic_EndlessRunnerPOE : MonoBehaviour
{
    [Header("References")]
    public GameObject bossPrefab;
    public Transform player;
    public BossEnvironmentLogic bossEnvironment;
    public FloorSpawnerLogic floorSpawner;
    public HUDLogic_EndlessRunnerPOE hudLogic;

    [Header("Event Settings")]
    public int bossNumber = 1;

    [Header("Spawn Position")]
    public float spawnX = -8f;
    public float riseStartY = -4f;
    public float finalBossY = 2f;
    public float spawnDistanceAhead = -20f;

    public bool BossPhaseInProgress { get; private set; }
    public bool BossSpawned { get; private set; }
    public bool BossPhaseFinished { get; private set; }

    private GameObject currentBoss;

    void Start()
    {
        if (hudLogic == null)
        {
            hudLogic = Object.FindFirstObjectByType<HUDLogic_EndlessRunnerPOE>();
        }
    }

    public void TriggerBossRise()
    {
        Debug.Log("BossSpawner: TriggerBossRise called.");

        if (BossSpawned)
        {
            Debug.Log("BossSpawner: Boss already spawned.");
            return;
        }

        if (BossPhaseFinished)
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

        LevelBossTwo_Logic_EndlessRunnerPOE bossTwoLogic =
    currentBoss.GetComponent<LevelBossTwo_Logic_EndlessRunnerPOE>();

        if (bossTwoLogic != null)
        {
            bossTwoLogic.player = player;
            bossTwoLogic.startY = riseStartY;
            bossTwoLogic.finalY = finalBossY;
            bossTwoLogic.BeginRise();
        }
        else
        {
            LevelBossOne_Logic_EndlessRunnerPOE bossOneLogic =
                currentBoss.GetComponent<LevelBossOne_Logic_EndlessRunnerPOE>();

            if (bossOneLogic != null)
            {
                bossOneLogic.player = player;
                bossOneLogic.startY = riseStartY;
                bossOneLogic.finalY = finalBossY;
                bossOneLogic.BeginRise();
            }
            else
            {
                Debug.LogError("BossSpawner: Spawned boss is missing a supported boss logic script.");
            }
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

        BossSpawned = true;
        BossPhaseInProgress = true;

        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.BossSpawned(bossNumber);
        }

        if (hudLogic != null)
        {
            hudLogic.UpdateBossStatus("Boss: Active");
        }

        Debug.Log("BossSpawner: Boss started. Water mode ON.");
    }

    public void EndBossFight()
    {
        if (BossPhaseFinished)
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

        BossSpawned = false;
        BossPhaseInProgress = false;
        BossPhaseFinished = true;

        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.BossBeaten(bossNumber);
        }

        if (hudLogic != null)
        {
            hudLogic.UpdateBossStatus("Boss: Complete");
        }
    }
}