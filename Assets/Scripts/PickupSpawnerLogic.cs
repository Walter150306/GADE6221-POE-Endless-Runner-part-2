using UnityEngine;

public class PickupSpawnerLogic : MonoBehaviour
{
    [Header("Pickup Prefabs")]
    public GameObject shieldPickupPrefab;
    public GameObject doublePointsPickupPrefab;
    public GameObject invulnerabilityPickupPrefab;

    [Header("Spawn Timing")]
    public float spawnInterval = 3f;
    private float timer = 0f;

    [Header("Lane Positions - Player View")]
    public float farRightViewLaneX = -11f;
    public float rightViewLaneX = -8.7f;
    public float middleViewLaneX = -6.7f;
    public float leftViewLaneX = -4.8f;
    public float farLeftViewLaneX = -2f;

    [Header("Spawn Position")]
    public float spawnY = 0.9f;
    public float spawnZ = -40f;

    [Header("Base Spawn Chance")]
    [Range(0f, 1f)]
    public float shieldSpawnChance = 0.4f;

    [Header("Relative Spawn Chances")]
    [Tooltip("Fish is 50% less likely than shield, so this should be 0.5")]
    [Range(0f, 1f)]
    public float doublePointsMultiplier = 0.5f;

    [Tooltip("Snowflake is 70% less likely than shield, so this should be 0.3")]
    [Range(0f, 1f)]
    public float invulnerabilityMultiplier = 0.3f;

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
        float doublePointsSpawnChance = shieldSpawnChance * doublePointsMultiplier;
        float invulnerabilitySpawnChance = shieldSpawnChance * invulnerabilityMultiplier;

        float totalChance =
            shieldSpawnChance +
            doublePointsSpawnChance +
            invulnerabilitySpawnChance;

        float roll = Random.value;

        if (roll > totalChance)
        {
            return;
        }

        GameObject pickupToSpawn = ChoosePickupFromRoll(
            roll,
            shieldSpawnChance,
            doublePointsSpawnChance,
            invulnerabilitySpawnChance
        );

        if (pickupToSpawn == null)
        {
            Debug.LogWarning("PickupSpawner: Tried to spawn a pickup, but its prefab is missing.");
            return;
        }

        float spawnX = GetRandomLaneX();
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

        Instantiate(pickupToSpawn, spawnPosition, Quaternion.identity);
    }

    GameObject ChoosePickupFromRoll(
        float roll,
        float shieldChance,
        float doublePointsChance,
        float invulnerabilityChance
    )
    {
        if (roll <= invulnerabilityChance)
        {
            return invulnerabilityPickupPrefab;
        }

        if (roll <= invulnerabilityChance + doublePointsChance)
        {
            return doublePointsPickupPrefab;
        }

        return shieldPickupPrefab;
    }

    float GetRandomLaneX()
    {
        int lane = Random.Range(0, 5);

        if (lane == 0)
        {
            return farRightViewLaneX;
        }

        if (lane == 1)
        {
            return rightViewLaneX;
        }

        if (lane == 2)
        {
            return middleViewLaneX;
        }

        if (lane == 3)
        {
            return leftViewLaneX;
        }

        return farLeftViewLaneX;
    }
}