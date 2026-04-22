using UnityEngine;

public class BossEntranceTileLogic : MonoBehaviour
{
    public float triggerDistance = 25f;

    private Transform player;
    private BossSpawnerLogic_EndlessRunnerPOE bossSpawner;
    private Transform triggerTarget;
    private bool hasTriggered = false;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("BossEntranceTileLogic: Player not found. Make sure the player tag is Player.");
        }

        bossSpawner = FindFirstObjectByType<BossSpawnerLogic_EndlessRunnerPOE>();

        if (bossSpawner == null)
        {
            Debug.LogError("BossEntranceTileLogic: BossSpawnerLogic_EndlessRunnerPOE not found in scene.");
        }

        Transform movingTile = transform.Find("MovingTile");

        if (movingTile != null)
        {
            triggerTarget = movingTile;
        }
        else
        {
            triggerTarget = transform;
        }
    }

    void Update()
    {
        if (hasTriggered || player == null || bossSpawner == null || triggerTarget == null)
        {
            return;
        }

        float zDifference = Mathf.Abs(player.position.z - triggerTarget.position.z);

        if (zDifference <= triggerDistance)
        {
            Debug.Log("BossEntranceTileLogic: Player reached boss entrance tile. Starting boss.");
            bossSpawner.TriggerBossRise();
            hasTriggered = true;
        }
    }
}