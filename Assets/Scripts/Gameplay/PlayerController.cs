using System;
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
    [SerializeField] private float mouseSensitivity = 200f;
    [SerializeField] private Rigidbody rb;
    
    private Camera playerCamera;
    private float mouseX;
    private float mouseY;
    private float moveX;
    private float moveZ;

    private bool _canPlayerMove = true;
    public InputManager InputManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        PlayerMovement();
        Vector3 moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }

    public virtual void PlayerMovement()
    {
        if (!_canPlayerMove) return;
        CalculateMouseAndCam();
    }

    public void ToggleMovement(bool movementEnabled)
    {
        _canPlayerMove = movementEnabled;
        Debug.Log(movementEnabled);
    }

    private void CalculateMouseAndCam()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(0f, mouseX, 0f);
        var ClampedY = Mathf.Clamp(mouseY, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(-ClampedY, 0f, 0f);
    }

    public void CalculateMovement(Vector2 playerdirection)
    {
        moveX = playerdirection.x; // A/D or Left/Right
        moveZ = playerdirection.y;   // W/S or Up/Down
    }
    
}
