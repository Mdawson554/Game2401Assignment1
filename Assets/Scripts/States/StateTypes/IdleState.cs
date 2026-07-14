using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States.StateTypes
{
    public class IdleState : IStates
    {
        /// <summary>
        /// a transitional state that enables or disables input 
        /// </summary>
        private PlayerStateMachine playerStateMachine;
        public IdleState(PlayerStateMachine stateMachine)
        {
            playerStateMachine = stateMachine;
        }
        
        public void EnterState()
        {
            playerStateMachine.playerController.InputManager.EnableMoveInput(true);
            //enable pause input, dialogue input through interact and pause input.
        }

        private void InternalStateChange(InputAction.CallbackContext obj)
        {
            playerStateMachine.changeState(playerStateMachine.walkstate);
        }
        
        public void ExecuteState()
        {
            //listen for inp
            //if (movement) go to movement state
            //if interact go to the dialogue state
            //if pause go to the pause state
        }
        
        
        public void ExitState()
        {
            playerStateMachine.playerController.InputManager.playerInputActions.Player.Move.started -= InternalStateChange;
            //transition out of this state into another
        }
    }
}
