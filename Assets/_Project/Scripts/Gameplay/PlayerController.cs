using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float startSpeed = 1f;
        [SerializeField] private float acceleration = 6f;

        [Header("Mouse Look")]
        [SerializeField] private float mouseSensitivity = 2f;

        [SerializeField] private float DialogueClampMin;
        [SerializeField] private float DialogueClamMax;

        private bool skipNextIdleCameraFrame = false;
        private Rigidbody rb;
        private Camera playerCamera;

        private Vector2 moveInput;
        private Vector2 lookInput;

        private float yaw;
        private float pitch;

        private float currentSpeed = 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            playerCamera = GetComponentInChildren<Camera>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currentSpeed = startSpeed;
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

            bool isMoving = movement.sqrMagnitude > 0.01f;

            if (isMoving)
            {
                currentSpeed = Mathf.MoveTowards(
                    currentSpeed,
                    moveSpeed,
                    acceleration * Time.deltaTime
                );
            }
            else
            {
                currentSpeed = startSpeed;
            }

            movement.Normalize();

            Vector3 velocity = movement * currentSpeed;
            velocity.y = rb.linearVelocity.y;

            rb.linearVelocity = velocity;
        }

        public void CalculateMouseAndCam()
        {
            if (skipNextIdleCameraFrame)
            {
                skipNextIdleCameraFrame = false;
                return; 
            }
            yaw += lookInput.x * mouseSensitivity;
            pitch -= lookInput.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -80f, 80f);
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        
        public void CalculateMouseAndCamDialogue() //so that way the interactor edge case is less likely to happen.
        {
            yaw += lookInput.x * mouseSensitivity;
            pitch -= lookInput.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, DialogueClampMin, DialogueClamMax);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private float NormalizeAngle(float angle)
        {
            if (angle > 180f)
                angle -= 360f;
            return angle;
        }
        
        public void SyncIdleToCurrentCamera()
        {
            var camRot = playerCamera.transform.rotation.eulerAngles;
            yaw = camRot.y;
            pitch = NormalizeAngle(camRot.x); 
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        public void SetSkipNextIdleCameraFrame(bool skip)
        {
            skipNextIdleCameraFrame = skip;
        }
    }
}