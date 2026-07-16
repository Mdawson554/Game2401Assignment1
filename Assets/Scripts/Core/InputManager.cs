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
            playerInputActions.Player.Next.performed += OnNext;
        }

        private void OnDisable()
        {
            playerInputActions.Disable();
            playerInputActions.Player.Move.started -= OnMove;
            playerInputActions.Player.Move.canceled -= OnMove;
            playerInputActions.Player.Pause.performed -= OnPause;
            playerInputActions.Player.Interact.performed -= Interact;
            playerInputActions.Player.Next.performed -= OnNext;
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
        
    
        public void OnPause(InputAction.CallbackContext context)
        {
           _playerStateMachine.Pause();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _playerController.CalculatePlayerMovement(context.ReadValue<Vector2>());
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            Debug.Log("Next");
            DialogueManager.Instance.NextDialogue();
        }
        
        private void Interact(InputAction.CallbackContext context)
        {
            Debug.Log("OnInteract");
            UIManager.Instance.HideToastPrompt();
            _playerStateMachine.playerInteractor.CurrentInteractable?.OnInteract();
            _playerStateMachine.playerInteractor.CurrentInteractable = null;

        }
    }
}

/*using UnityEngine;
using UnityEngine.InputSystem;
using Core;

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
        playerInputActions = new PlayerInputActions();
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

    private void Interact(InputAction.CallbackContext context)
    {
        _playerStateMachine.playerInteractor.InteractWithObject();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        _playerStateMachine.Pause();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _playerController.CalculateMovement(context.ReadValue<Vector2>());
    }
}*/
