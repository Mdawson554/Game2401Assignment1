using States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class InputManager : MonoBehaviour
    {
        private PlayerStateMachine _playerStateMachine;
        private PlayerController _playerController;

        public PlayerInputActions playerInputActions;

        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
        }

        private void Start()
        {
            _playerStateMachine = GameManager.Instance.playerStateMachine;
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            playerInputActions.Enable();
            playerInputActions.Player.Move.performed += OnMove;
            playerInputActions.Player.Move.canceled += OnMove;
            playerInputActions.Player.Look.performed += OnLook;
            playerInputActions.Player.Look.canceled += OnLook;
            playerInputActions.Player.Pause.performed += OnPause;
            playerInputActions.Player.Interact.performed += Interact;
            playerInputActions.Player.Next.performed += OnNext;
        }

        private void OnDisable()
        {
            playerInputActions.Player.Move.performed -= OnMove;
            playerInputActions.Player.Move.canceled -= OnMove;
            playerInputActions.Player.Look.performed -= OnLook;
            playerInputActions.Player.Look.canceled -= OnLook;
            playerInputActions.Player.Pause.performed -= OnPause;
            playerInputActions.Player.Interact.performed -= Interact;
            playerInputActions.Player.Next.performed -= OnNext;

            playerInputActions.Disable();
        }
        
        public void EnableMoveInput(bool enabled)
        {
            if (enabled)
            {
                playerInputActions.Player.Move.Enable();
            }
            else
            {
                playerInputActions.Player.Move.Disable();
                _playerController.SetMoveInput(Vector2.zero);
            }
        }

        public void EnableLookInput(bool enabled)
        {
            if (enabled)
            {
                playerInputActions.Player.Look.Enable();
            }
            else
            {
                playerInputActions.Player.Look.Disable();

                _playerController.SetLookInput(Vector2.zero);
            }
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _playerController.SetMoveInput(context.ReadValue<Vector2>());
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            _playerController.SetLookInput(context.ReadValue<Vector2>());
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            _playerStateMachine.Pause();
        }

        private void OnNext(InputAction.CallbackContext context)
        {
            DialogueManager.Instance.NextDialogue();
        }

        private void Interact(InputAction.CallbackContext context)
        {
            UIManager.Instance.HideToastPrompt();

            _playerStateMachine.playerInteractor.CurrentInteractable?.OnInteract();
            _playerStateMachine.playerInteractor.CurrentInteractable = null;
        }
    }
}