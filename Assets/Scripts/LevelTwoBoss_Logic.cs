using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBossTwo_Logic_EndlessRunnerPOE : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject warningPrefab;
    public GameObject spikePrefab;

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

    [Header("Spike Attack Settings")]
    public float attackInterval = 4f;
    public float warningDuration = 2f;
    public int lanesPerAttack = 3;

    [Header("Attack Spawn Position")]
    public float warningY = 0.78f;
    public float spikeY = 0.72f;
    public float attackZOffsetFromPlayer = 0f;

    [Header("Slam Animation")]
    public float slamDownDistance = 2f;
    public float slamDownTime = 0.18f;
    public float slamReturnTime = 0.35f;

    [Header("Lane Positions - Player View")]
    public float farRightViewLaneX = -11f;
    public float rightViewLaneX = -8.7f;
    public float middleViewLaneX = -6.7f;
    public float leftViewLaneX = -4.8f;
    public float farLeftViewLaneX = -2f;

    private float attackTimer = 0f;
    private bool isAttacking = false;

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

        if (hasFullyRisen && !isAttacking)
        {
            HandleMovement();
            HandleAttackTimer();
        }
    }

    public void BeginRise()
    {
        isRising = true;
        hasFullyRisen = false;
        isAttacking = false;
        attackTimer = 0f;

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

    void HandleAttackTimer()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            attackTimer = 0f;
            StartCoroutine(SpikeSlamAttack());
        }
    }

    IEnumerator SpikeSlamAttack()
    {
        isAttacking = true;

        List<int> attackLanes = ChooseAttackLanes();

        float attackZ = player.position.z + attackZOffsetFromPlayer;

        List<GameObject> warningMarkers = new List<GameObject>();

        foreach (int laneIndex in attackLanes)
        {
            Vector3 warningPosition = new Vector3(GetLaneX(laneIndex), warningY, attackZ);

            if (warningPrefab != null)
            {
                GameObject warning = Instantiate(warningPrefab, warningPosition, Quaternion.identity);
                warningMarkers.Add(warning);
            }
        }

        yield return new WaitForSeconds(warningDuration);

        foreach (GameObject warning in warningMarkers)
        {
            if (warning != null)
            {
                Destroy(warning);
            }
        }

        yield return StartCoroutine(SlamBoss());

        foreach (int laneIndex in attackLanes)
        {
            Vector3 spikePosition = new Vector3(GetLaneX(laneIndex), spikeY, attackZ);

            if (spikePrefab != null)
            {
                Instantiate(spikePrefab, spikePosition, Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
    }

    IEnumerator SlamBoss()
    {
        Vector3 startPosition = transform.position;
        Vector3 downPosition = new Vector3(startPosition.x, finalY - slamDownDistance, lockedZ);

        float timer = 0f;

        while (timer < slamDownTime)
        {
            timer += Time.deltaTime;
            float t = timer / slamDownTime;
            transform.position = Vector3.Lerp(startPosition, downPosition, t);
            yield return null;
        }

        transform.position = downPosition;

        timer = 0f;

        while (timer < slamReturnTime)
        {
            timer += Time.deltaTime;
            float t = timer / slamReturnTime;
            transform.position = Vector3.Lerp(downPosition, startPosition, t);
            yield return null;
        }

        transform.position = startPosition;
    }

    List<int> ChooseAttackLanes()
    {
        List<int> lanes = new List<int> { 0, 1, 2, 3, 4 };

        for (int i = 0; i < lanes.Count; i++)
        {
            int randomIndex = Random.Range(i, lanes.Count);
            int temp = lanes[i];
            lanes[i] = lanes[randomIndex];
            lanes[randomIndex] = temp;
        }

        int clampedLaneCount = Mathf.Clamp(lanesPerAttack, 1, 4);

        return lanes.GetRange(0, clampedLaneCount);
    }

    float GetLaneX(int laneIndex)
    {
        if (laneIndex == 0)
        {
            return farRightViewLaneX;
        }

        if (laneIndex == 1)
        {
            return rightViewLaneX;
        }

        if (laneIndex == 2)
        {
            return middleViewLaneX;
        }

        if (laneIndex == 3)
        {
            return leftViewLaneX;
        }

        return farLeftViewLaneX;
    }
}