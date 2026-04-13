using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PickUpLogic_EndlessRunnerPOE : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupSpeed = 5f;

    [Header("Bobbing Settings")]
    public float bobHeight = 0.2f;
    public float bobSpeed = 3f;

    private float baseY;
    private Collider pickupCollider;
    private Rigidbody pickupRigidbody;

    void Awake()
    {
        pickupCollider = GetComponent<Collider>();
        pickupRigidbody = GetComponent<Rigidbody>();

        pickupCollider.isTrigger = true;
        pickupRigidbody.isKinematic = true;
        pickupRigidbody.useGravity = false;
    }

    void Start()
    {
        baseY = transform.position.y;
    }

    void Update()
    {
        MovePickup();
        BobbingEffect();
    }

    private void MovePickup()
    {
        transform.position += Vector3.forward * pickupSpeed * Time.deltaTime;
    }

    private void BobbingEffect()
    {
        float newY = baseY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManagerLogic_EndlessRunner.instance != null)
            {
                GameManagerLogic_EndlessRunner.instance.ActivateShield();
            }

            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Deleter"))
        {
            Destroy(gameObject);
        }
    }
}