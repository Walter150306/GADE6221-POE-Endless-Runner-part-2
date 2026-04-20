using UnityEngine;

public class BossEnvironmentLogic : MonoBehaviour
{
    [Header("Boss Visuals")]
    public GameObject underwaterVolume;
    public GameObject bubbleParticles;

    public void EnterBossPhase()
    {
        if (underwaterVolume != null)
            underwaterVolume.SetActive(true);

        if (bubbleParticles != null)
            bubbleParticles.SetActive(true);
    }

    public void ExitBossPhase()
    {
        if (underwaterVolume != null)
            underwaterVolume.SetActive(false);

        if (bubbleParticles != null)
            bubbleParticles.SetActive(false);
    }
}