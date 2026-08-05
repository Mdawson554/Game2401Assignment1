using States;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Core
{
    public class InputManager : MonoBehaviour
    {
        private PlayerStateMachine _playerStateMachine;
        private PlayerController _playerController;
        private PlayerInputActions _playerInputActions;
        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
        }
        private void Start()
        {
            _playerStateMachine = GameManager.Instance.playerStateMachine;
            _playerController = GetComponent<PlayerController>();
        }
        private void OnEnable()
        {
            _playerInputActions.Enable();
            _playerInputActions.Player.Move.performed += OnMove;
            _playerInputActions.Player.Move.canceled += OnMove;
            _playerInputActions.Player.Look.performed += OnLook;
            _playerInputActions.Player.Look.canceled += OnLook;
            _playerInputActions.Player.Pause.performed += OnPause;
            _playerInputActions.Player.Interact.performed += Interact;
            _playerInputActions.Player.Next.performed += OnNext;
        }
        private void OnDisable()
        {
            _playerInputActions.Player.Move.performed -= OnMove;
            _playerInputActions.Player.Move.canceled -= OnMove;
            _playerInputActions.Player.Look.performed -= OnLook;
            _playerInputActions.Player.Look.canceled -= OnLook;
            _playerInputActions.Player.Pause.performed -= OnPause;
            _playerInputActions.Player.Interact.performed -= Interact;
            _playerInputActions.Player.Next.performed -= OnNext;
            _playerInputActions.Disable();
        }
        
        public void EnableMoveInput(bool enabled)
        {
            if (enabled)
            {
                _playerInputActions.Player.Move.Enable();
            }
            else
            {
                _playerInputActions.Player.Move.Disable();
                _playerController.SetMoveInput(Vector2.zero);
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
            if (_playerStateMachine.currentState != _playerStateMachine.dialoguestate)
            {
                UIManager.Instance.HideToastPrompt();
            }
            
            _playerStateMachine.playerInteractor.CurrentInteractable?.OnInteract();
            _playerStateMachine.playerInteractor.CurrentInteractable = null;
        }
    }
}