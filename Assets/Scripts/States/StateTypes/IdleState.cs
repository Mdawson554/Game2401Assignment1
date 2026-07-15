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
            playerStateMachine.inputManager.EnableMoveInput(true);
            
            //enable pause input, dialogue input through interact and pause input.
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
            //transition out of this state into another
        }
        public void Update()
        {
            playerStateMachine.playerController.PlayerMovement();
            playerStateMachine.playerController.CalculateMouseAndCam();
        }
    }
}
