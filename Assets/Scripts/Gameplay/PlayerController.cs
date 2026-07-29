using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 2f;

    private Rigidbody rb;
    private Camera playerCamera;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float yaw;
    private float pitch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }
    public void SetLookInput(Vector2 input)
    {
        lookInput = input;
    }
    public void PlayerMovement()
    {
        Vector3 movement =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        movement.Normalize();

        Vector3 velocity = movement * moveSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }
    public void CalculateMouseAndCam()
    {
        yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -80f, 80f);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}