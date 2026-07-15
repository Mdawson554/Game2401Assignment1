using Core;
using States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 2f; 
    [SerializeField] private Rigidbody rb;
    
    private Camera playerCamera;
    private float mouseX;
    private float mouseY;
    private float moveX;
    private float moveZ;
    private bool _canPlayerMove = true;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseX = 0f;
        mouseY = 0f;
    }
    
    private void Update()
    {
       
    }
    
    public virtual void PlayerMovement()
    {
        Vector3 moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
        //CalculateMouseAndCam();
    }
    
    public void CalculateMouseAndCam()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);
        transform.localRotation = Quaternion.Euler(0f, mouseX, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(-mouseY, 0f, 0f);
    }
    
    public void CalculatePlayerMovement(Vector2 playerdirection)
    {
        moveX = playerdirection.x; // A/D or Left/Right
        moveZ = playerdirection.y; // W/S or Up/Down
    }
}
