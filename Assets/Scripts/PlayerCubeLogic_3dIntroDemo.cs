using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCubeLogic_3dIntroDemo : MonoBehaviour
{
    public Rigidbody PlayerRigidbody;
    public float moveX;
    public float moveZ;

    [Header("Player Movement Settings")]
    public float playerSpeed = 1f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    private bool isGrounded;

    [Header("Boss Water Mode")]
    public bool isInBossWaterPhase = false;
    public float waterSurfaceY = 0.72f;
    private bool isDiving = false;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, 0f) * playerSpeed * Time.deltaTime;
        PlayerRigidbody.transform.Translate(movement);

        if (!isInBossWaterPhase) //Notes from Baf: Make so that this only activates when touching the boss floor
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                PlayerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void EnterBossWaterPhase()
    {
        isInBossWaterPhase = true;

        Vector3 pos = transform.position;
        pos.y = waterSurfaceY;
        transform.position = pos;

        PlayerRigidbody.linearVelocity = Vector3.zero;
    }

    public void ExitBossWaterPhase()
    {
        isInBossWaterPhase = false;
        isDiving = false;
    }
}