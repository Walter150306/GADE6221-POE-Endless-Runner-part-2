using UnityEngine;

public class BarrelLogic : MonoBehaviour
{
    [Header("Barrel Settings")]
    public Transform player;
    public float barrelSpeed = 5f;
    public float barrelRotationSpeed = 100f;

    private bool hasPassedPlayer = false;

    void Update()
    {
        MoveBarrel();
        CheckIfPassedPlayer();
    }

    private void MoveBarrel()
    {
        transform.Translate(Vector3.forward * barrelSpeed * Time.deltaTime);
    }

    private void CheckIfPassedPlayer()
    {
        if (player == null || hasPassedPlayer)
        {
            return;
        }

        if (transform.position.z > player.position.z)
        {
            hasPassedPlayer = true;

            if (GameManagerLogic_EndlessRunner.instance != null)
            {
                GameManagerLogic_EndlessRunner.instance.AddScore(1);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManagerLogic_EndlessRunner.instance != null)
            {
                GameManagerLogic_EndlessRunner.instance.PlayerGetsHit();
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Deleter"))
        {
            Destroy(gameObject);
        }
    }
}