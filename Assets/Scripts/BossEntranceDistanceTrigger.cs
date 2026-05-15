using UnityEngine;

public class BossEntranceDistanceTrigger : MonoBehaviour
{
    public float triggerDistance = 8f;

    private Transform player;
    private BossSpawnerLogic_EndlessRunnerPOE bossSpawner;
    private bool hasTriggered = false;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        bossSpawner = FindFirstObjectByType<BossSpawnerLogic_EndlessRunnerPOE>();

        if (player == null)
        {
            Debug.LogError("BossEntranceDistanceTrigger: Could not find Player.");
        }

        if (bossSpawner == null)
        {
            Debug.LogError("BossEntranceDistanceTrigger: Could not find BossSpawnerLogic_EndlessRunnerPOE.");
        }
    }

    void Update()
    {
        if (hasTriggered || player == null || bossSpawner == null)
        {
            return;
        }

        float zDifference = Mathf.Abs(player.position.z - transform.position.z);

        if (zDifference <= triggerDistance)
        {
            Debug.Log("BossEntranceDistanceTrigger: Player reached boss entrance tile.");
            bossSpawner.TriggerBossRise();
            hasTriggered = true;
        }
    }
}