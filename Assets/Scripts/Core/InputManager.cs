using States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class InputManager : MonoBehaviour
    {
        private PlayerStateMachine _playerStateMachine;
        private PlayerInputActions _playerInputActions;

        private void Start()
        {
            _playerStateMachine = GameManager.Instance.playerStateMachine;
            _playerInputActions = GetComponent<PlayerInputActions>();
        }

        private void OnEnable()
        {
        
            _playerInputActions.Enable();
            _playerInputActions.Player.Pause.performed += OnPause;
            _playerInputActions.Player.Interact.performed += OnInteract;
        }

        private void OnDisable()
        {
            _playerInputActions.Disable();
            _playerInputActions.Player.Pause.performed -= OnPause;
            _playerInputActions.Player.Interact.performed -= OnInteract;
        }
    
        public void OnPause(InputAction.CallbackContext context)
        {
            GameManager.Instance.Pause();
            _playerStateMachine.changeState(_playerStateMachine.PauseState);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _playerStateMachine.changeState(_playerStateMachine.walkstate);
        }
        private void OnInteract(InputAction.CallbackContext context)
        {
            Debug.Log("OnInteract");
            UIManager.Instance.HideToastPrompt();
            _playerStateMachine.playerInteractor.Interactable?.OnInteract();
            _playerStateMachine.changeState(_playerStateMachine.dialoguestate);
        }
    }
}
