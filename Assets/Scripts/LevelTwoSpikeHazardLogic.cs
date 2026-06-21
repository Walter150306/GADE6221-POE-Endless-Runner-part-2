using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class LevelTwoSpikeHazardLogic : MonoBehaviour
{
    [Header("Spike Movement")]
    public float hiddenDepth = 2f;
    public float riseSpeed = 12f;
    public float activeDuration = 0.8f;
    public float sinkSpeed = 10f;

    private Collider spikeCollider;
    private Rigidbody spikeRigidbody;
    private bool hasHitPlayer = false;

    void Awake()
    {
        spikeCollider = GetComponent<Collider>();
        spikeRigidbody = GetComponent<Rigidbody>();

        spikeCollider.isTrigger = true;

        spikeRigidbody.isKinematic = true;
        spikeRigidbody.useGravity = false;
    }

    IEnumerator Start()
    {
        Vector3 visiblePosition = transform.position;
        Vector3 hiddenPosition = visiblePosition + Vector3.down * hiddenDepth;

        transform.position = hiddenPosition;

        while (Vector3.Distance(transform.position, visiblePosition) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                visiblePosition,
                riseSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = visiblePosition;

        yield return new WaitForSeconds(activeDuration);

        while (Vector3.Distance(transform.position, hiddenPosition) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                hiddenPosition,
                sinkSpeed * Time.deltaTime
            );

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHitPlayer)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            hasHitPlayer = true;

            if (GameManagerLogic_EndlessRunner.instance != null)
            {
                GameManagerLogic_EndlessRunner.instance.PlayerGetsHit();
            }
        }
    }
}