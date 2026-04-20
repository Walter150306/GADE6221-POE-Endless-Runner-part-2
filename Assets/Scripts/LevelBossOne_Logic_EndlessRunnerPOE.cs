using UnityEngine;

public class LevelBossOne_Logic_EndlessRunnerPOE : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Follow Settings")]
    public float fixedDistanceAhead = 20f;
    public float fixedHeight = 3f;
    public float centerX = -8f;

    [Header("Floating Settings")]
    public float bobHeight = 0.2f;
    public float bobSpeed = 3f;

    [Header("Side To Side Settings")]
    public float sideToSideAmplitude = 2f;
    public float sideToSideFrequency = 1f;

    void Update()
    {
        if (player == null)
        {
            return;
        }

        float newX = centerX + Mathf.Sin(Time.time * sideToSideFrequency) * sideToSideAmplitude;
        float newY = fixedHeight + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        float newZ = player.position.z + fixedDistanceAhead;

        transform.position = new Vector3(newX, newY, newZ);
    }
}