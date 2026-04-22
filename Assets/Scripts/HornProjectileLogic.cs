using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class HornProjectileLogic : MonoBehaviour
{
    [Header("Horn Settings")]
    public float hornSpeed = 12f;
    public float lifetime = 5f;

    private Collider hornCollider;
    private Rigidbody hornRigidbody;

    void Awake()
    {
        hornCollider = GetComponent<Collider>();
        hornRigidbody = GetComponent<Rigidbody>();

        hornCollider.isTrigger = true;
        hornRigidbody.isKinematic = true;
        hornRigidbody.useGravity = false;
    }

    void Start()
    {
        // Make the horn face forward (optional, just visual)
        transform.rotation = Quaternion.LookRotation(Vector3.forward);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move straight along +Z axis (world space)
        transform.position += transform.forward * hornSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManagerLogic_EndlessRunner.instance != null)
            {
                GameManagerLogic_EndlessRunner.instance.PlayerGetsHit();
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