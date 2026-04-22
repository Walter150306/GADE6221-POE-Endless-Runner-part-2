using UnityEngine;

public class LevelBossOne_Logic_EndlessRunnerPOE : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform hornSpawnPoint;
    public GameObject hornProjectilePrefab;

    [Header("Boss Position")]
    public float lockedZ;
    public float centerX = -8f;
    public float startY = -4f;
    public float finalY = 2f;

    [Header("Rise Settings")]
    public float riseSpeed = 4f;
    private bool isRising = false;
    private bool hasFullyRisen = false;

    [Header("Floating Settings")]
    public float bobHeight = 0.2f;
    public float bobSpeed = 3f;

    [Header("Side To Side Settings")]
    public float sideToSideAmplitude = 3f;
    public float sideToSideFrequency = 1f;

    [Header("Attack Settings")]
    public float shootInterval = 1f;
    private float shootTimer = 0f;

    void Start()
    {
        lockedZ = transform.position.z;
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        if (isRising && !hasFullyRisen)
        {
            HandleRise();
            return;
        }

        if (hasFullyRisen)
        {
            HandleMovement();
            HandleShooting();
        }
    }

    public void BeginRise()
    {
        isRising = true;
        hasFullyRisen = false;

        Vector3 pos = transform.position;
        pos.y = startY;
        transform.position = pos;
    }

    void HandleRise()
    {
        Vector3 pos = transform.position;
        pos.y += riseSpeed * Time.deltaTime;

        if (pos.y >= finalY)
        {
            pos.y = finalY;
            isRising = false;
            hasFullyRisen = true;
        }

        transform.position = pos;
    }

    void HandleMovement()
    {
        float newX = centerX + Mathf.Sin(Time.time * sideToSideFrequency) * sideToSideAmplitude;
        float newY = finalY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        transform.position = new Vector3(newX, newY, lockedZ);
    }

    void HandleShooting()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            ShootHorn();
            shootTimer = 0f;
        }
    }

    void ShootHorn()
    {
        if (hornProjectilePrefab == null || hornSpawnPoint == null)
        {
            return;
        }

        Instantiate(hornProjectilePrefab, hornSpawnPoint.position, hornSpawnPoint.rotation);
    }
}