using UnityEngine;

public class BossExitTileLogic : MonoBehaviour
{
    private bool triggered = false;
    private BossSpawnerLogic_EndlessRunnerPOE bossSpawner;

    void Start()
    {
        bossSpawner = Object.FindFirstObjectByType<BossSpawnerLogic_EndlessRunnerPOE>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
        {
            return;
        }

        PlayerCubeLogic_3dIntroDemo playerLogic =
            other.GetComponent<PlayerCubeLogic_3dIntroDemo>();

        if (playerLogic == null)
        {
            playerLogic = other.GetComponentInParent<PlayerCubeLogic_3dIntroDemo>();
        }

        if (playerLogic == null && !other.CompareTag("Player"))
        {
            return;
        }

        triggered = true;

        if (bossSpawner == null)
        {
            bossSpawner = Object.FindFirstObjectByType<BossSpawnerLogic_EndlessRunnerPOE>();
        }

        if (bossSpawner != null)
        {
            Debug.Log("BossExitTileLogic: Player hit boss exit trigger.");
            bossSpawner.EndBossFight();
        }
        else
        {
            Debug.LogError("BossExitTileLogic: Could not find BossSpawnerLogic_EndlessRunnerPOE.");
        }
    }
}