using States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class InputManager : MonoBehaviour
    {
        private PlayerStateMachine _playerStateMachine;
        private PlayerInputActions playerInputActions;
        private PlayerController _playerController;

        private void Start()
        {
            _playerStateMachine = GameManager.Instance.playerStateMachine;
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            playerInputActions = new  PlayerInputActions();
            playerInputActions.Enable();
            playerInputActions.Player.Move.started += OnMove;
            playerInputActions.Player.Move.canceled += OnMove;
            playerInputActions.Player.Pause.performed += OnPause;
            playerInputActions.Player.Interact.performed += Interact;
        }

        private void OnDisable()
        {
            playerInputActions.Disable();
            playerInputActions.Player.Move.started -= OnMove;
            playerInputActions.Player.Move.canceled -= OnMove;
            playerInputActions.Player.Pause.performed -= OnPause;
            playerInputActions.Player.Interact.performed -= Interact;
        }
    
        public void OnPause(InputAction.CallbackContext context)
        {
            GameManager.Instance.Pause();
            _playerStateMachine.changeState(_playerStateMachine.PauseState);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _playerController.CalculateMovement(context.ReadValue<Vector2>());
        }
        
        private void Interact(InputAction.CallbackContext context)
        {
            Debug.Log("OnInteract");
            UIManager.Instance.HideToastPrompt();
            _playerStateMachine.playerInteractor.Interactable?.OnInteract();
            _playerStateMachine.changeState(_playerStateMachine.dialoguestate);
        }
    }
}
