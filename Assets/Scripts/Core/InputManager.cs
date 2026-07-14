using States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class InputManager : MonoBehaviour
    {
        private PlayerStateMachine _playerStateMachine;
        public PlayerInputActions playerInputActions;
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
            playerInputActions.Player.Skip.performed += OnSkip;
        }

        private void OnDisable()
        {
            playerInputActions.Disable();
            playerInputActions.Player.Move.started -= OnMove;
            playerInputActions.Player.Move.canceled -= OnMove;
            playerInputActions.Player.Pause.performed -= OnPause;
            playerInputActions.Player.Interact.performed -= Interact;
            playerInputActions.Player.Skip.performed -= OnSkip;
        }

        public void EnableMoveInput(bool isEnabled)
        {
            switch(isEnabled)
            {
                case true:
                    playerInputActions.Player.Move.started += OnMove;
                    playerInputActions.Player.Move.canceled += OnMove;
                    break;
                case false:
                    playerInputActions.Player.Move.started -= OnMove;
                    playerInputActions.Player.Move.canceled -= OnMove;
                    break;
            }
        }
        
        
        //make one for enable pauseinput,idle and dialogue states
    
        public void OnPause(InputAction.CallbackContext context)
        {
           _playerStateMachine.Pause();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _playerController.CalculateMovement(context.ReadValue<Vector2>());
            //have another reference that calls the statemachine that calcules the movement but changes the state
        }

        public void OnSkip(InputAction.CallbackContext context)
        {
            DialogueManager.Instance.OnSkipButtonClicked();
        }
        
        private void Interact(InputAction.CallbackContext context)
        {
            Debug.Log("OnInteract");
            UIManager.Instance.HideToastPrompt();
            _playerStateMachine.playerInteractor.Interactable?.OnInteract();
        }
    }
}
