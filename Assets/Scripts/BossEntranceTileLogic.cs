using UnityEngine;

public class BossEntranceTileLogic : MonoBehaviour
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

        if (!other.CompareTag("Player"))
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
            bossSpawner.TriggerBossRise();
        }
        else
        {
            Debug.LogError("BossEntranceTileLogic: Could not find BossSpawnerLogic_EndlessRunnerPOE.");
        }
    }
}