using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float CamSensitivity = 0.01f;
    
    private CharacterController _characterController;
    private Rigidbody _rigidbody;
    private PlayerState _currentState; 
    private Quaternion _targetRotation;
    
    private Vector2 _lookInput;
    private Vector2 _moveInput;
    private Vector3 _moveDirection;
    private Vector3 _velocity;
    private float yaw;
    private float pitch;
    private float Yclamp;
    
    private void Start()
    {
        _currentState = PlayerState.EXPLORE;
        OnStateUpdated?.Invoke(_currentState);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        CalculateMouse();
        CalculateCamera();
    }
    
    public event Action<PlayerState> OnStateUpdated;
    
    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    private void CalculateMouse()
    {
        // Accumulate both axes correctly
        yaw   += _lookInput.x * CamSensitivity;
        pitch -= _lookInput.y * CamSensitivity; // subtract so mouse-up looks up
        pitch  = Mathf.Clamp(pitch, -90f, 90f);

        // Rotate player body horizontally
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Move relative to where the player is facing (yaw only, ignore pitch for XZ movement)
        Vector3 forward = Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;
        Vector3 right   = Quaternion.Euler(0f, yaw, 0f) * Vector3.right;

        Vector3 moveVector = (forward * _moveInput.y + right * _moveInput.x).normalized;
        transform.position += moveVector * (moveSpeed * Time.deltaTime);
    }

    private void CalculateCamera()
    {
        // Camera only tilts up/down locally; player body handles left/right
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
