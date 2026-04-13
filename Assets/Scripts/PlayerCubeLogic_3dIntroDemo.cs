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

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            PlayerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}