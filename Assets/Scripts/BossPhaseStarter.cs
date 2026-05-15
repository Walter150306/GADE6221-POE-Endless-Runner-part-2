using UnityEngine;

public class BossPhaseStarter : MonoBehaviour
{
    [Header("References")]
    public FloorSpawnerLogic floorSpawner;
    public HUDLogic_EndlessRunnerPOE hudLogic;

    [Header("Boss Timing")]
    public float bossStartDelay = 30f;

    private float timer = 0f;
    private bool startedBossPhase = false;

    void Start()
    {
        if (hudLogic == null)
        {
            hudLogic = Object.FindFirstObjectByType<HUDLogic_EndlessRunnerPOE>();
        }

        if (hudLogic != null)
        {
            hudLogic.UpdateBossStatus("Boss: Waiting");
        }
    }

    void FixedUpdate()
    {
        if (startedBossPhase)
        {
            return;
        }

        timer += Time.fixedDeltaTime;

        if (timer >= bossStartDelay)
        {
            if (floorSpawner != null)
            {
                floorSpawner.StartBossTunnelPhase();
                startedBossPhase = true;

                if (hudLogic != null)
                {
                    hudLogic.UpdateBossStatus("Boss: Incoming");
                }

                Debug.Log("BossPhaseStarter: Boss tunnel phase started using FixedUpdate timer.");
            }
        }
    }
}