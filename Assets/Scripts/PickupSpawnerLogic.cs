using UnityEngine;

public class PickupSpawnerLogic : MonoBehaviour
{
    [Header("Pickup Prefab")]
    public GameObject shieldPickupPrefab;
    public Transform player;

    [Header("Spawn Timing")]
    public float spawnInterval = 6f;
    private float timer = 0f;

    [Header("Lane Positions")]
    public float leftLaneX = -7.12f;
    public float middleLaneX = -8.12f;
    public float rightLaneX = -6.12f;

    [Header("Spawn Position")]
    public float spawnY = 0.9f;
    public float spawnZ = -40f;

    [Header("Spawn Chance")]
    [Range(0f, 1f)]
    public float spawnChance = 0.4f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            TrySpawnPickup();
            timer = 0f;
        }
    }

    void TrySpawnPickup()
    {
        if (shieldPickupPrefab == null)
        {
            Debug.LogWarning("Shield pickup prefab is not assigned.");
            return;
        }

        float roll = Random.value;

        if (roll > spawnChance)
        {
            return;
        }

        float spawnX = GetRandomLaneX();
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

        GameObject pickup = Instantiate(shieldPickupPrefab, spawnPosition, Quaternion.identity);

        PickUpLogic_EndlessRunnerPOE pickupLogic = pickup.GetComponent<PickUpLogic_EndlessRunnerPOE>();

        if (pickupLogic != null)
        {
            //pickupLogic.player = player;
        }
    }

    float GetRandomLaneX()
    {
        int lane = Random.Range(0, 3);

        if (lane == 0)
        {
            return leftLaneX;
        }
        else if (lane == 1)
        {
            return middleLaneX;
        }
        else
        {
            return rightLaneX;
        }
    }
}