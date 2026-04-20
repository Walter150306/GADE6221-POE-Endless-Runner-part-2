using System.Buffers.Text;
using Unity.VisualScripting;
using UnityEngine;

public class LevelBossOne_Logic_EndlessRunnerPOE : MonoBehaviour
{
    public float pickupSpeed = 5f;
    public float bobHeight = 0.2f;
    public float bobSpeed = 3f;
    public float sideToSideAmplitude = 5f;
    public float sideToSideFrequency = 1f;
   
    private float baseX;
    private float baseY;
    private Collider BossCollider;
    private GameObject LevelOneBoss;

    void Start()
    {
        LevelOneBoss = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        BobbingEffect();
        MoveSideToSide();
    }


    private void BobbingEffect()
    {
        float newY = baseY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void MoveSideToSide()
    {
        float newX = baseX + Mathf.Sin(Time.time * sideToSideFrequency) * sideToSideAmplitude;
        transform.position = new Vector3(newX  , transform.position.y , transform.position.z);
    }
}
