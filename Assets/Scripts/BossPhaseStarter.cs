using UnityEngine;

public class BossPhaseStarter : MonoBehaviour
{
    public FloorSpawnerLogic floorSpawner;
    public float bossStartDelay = 30f;

    private bool startedBossPhase = false;

    void Update()
    {
        if (startedBossPhase)
        {
            return;
        }

        if (Time.timeSinceLevelLoad >= bossStartDelay)
        {
            if (floorSpawner != null)
            {
                floorSpawner.StartBossTunnelPhase();
                startedBossPhase = true;
            }
        }
    }
}