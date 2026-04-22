using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCubeLogic_3dIntroDemo : MonoBehaviour
{
    [Header("References")]
    public Rigidbody PlayerRigidbody;

    [Header("Movement Settings")]
    public float playerSpeed = 6f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    private bool isGrounded;

    [Header("Boss Water Mode")]
    public bool isInBossWaterPhase = false;
    public float waterSurfaceY = 0.72f;

    [Header("Water Movement")]
    public float floatStrength = 4f;
    public float diveSpeed = 4f;
    public float waterJumpForce = 4f;
    public float maxSubmergeDepth = 1.5f;
    public float maxJumpHeightAboveSurface = 2f;

    private float moveX;
    private float verticalWaterVelocity = 0f;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // FIXED: correct left/right direction
        moveX = -Input.GetAxis("Horizontal");

        if (!isInBossWaterPhase)
        {
            HandleNormalMovement();
        }
        else
        {
            HandleWaterMovement();
        }
    }

    void HandleNormalMovement()
    {
        Vector3 movement = new Vector3(moveX, 0f, 0f) * playerSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            PlayerRigidbody.linearVelocity = new Vector3(
                PlayerRigidbody.linearVelocity.x,
                0f,
                PlayerRigidbody.linearVelocity.z
            );

            PlayerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void HandleWaterMovement()
    {
        Vector3 pos = transform.position;

        // Left / Right movement
        pos.x += moveX * playerSpeed * Time.deltaTime;

        // Dive (hold CTRL)
        if (Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed)
        {
            verticalWaterVelocity = -diveSpeed;
        }
        else
        {
            // Float toward surface
            float difference = waterSurfaceY - pos.y;
            verticalWaterVelocity = difference * floatStrength;
        }

        // Jump out of water (tap Space)
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            verticalWaterVelocity = waterJumpForce;
        }

        pos.y += verticalWaterVelocity * Time.deltaTime;

        // Clamp depth + height
        float minY = waterSurfaceY - maxSubmergeDepth;
        float maxY = waterSurfaceY + maxJumpHeightAboveSurface;

        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;

        // Keep physics stable in water
        PlayerRigidbody.linearVelocity = Vector3.zero;
        PlayerRigidbody.angularVelocity = Vector3.zero;
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
        isGrounded = false;
        verticalWaterVelocity = 0f;

        Vector3 pos = transform.position;
        pos.y = waterSurfaceY;
        transform.position = pos;

        PlayerRigidbody.linearVelocity = Vector3.zero;
        PlayerRigidbody.angularVelocity = Vector3.zero;
        PlayerRigidbody.useGravity = false;
    }

    public void ExitBossWaterPhase()
    {
        isInBossWaterPhase = false;
        verticalWaterVelocity = 0f;
        PlayerRigidbody.useGravity = true;
    }
}