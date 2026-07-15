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
        }
        
        public void ExitState()
        {
           
        }
        public void Update()
        {
            playerStateMachine.playerController.PlayerMovement();
            playerStateMachine.playerController.CalculateMouseAndCam();
        }
    }
}
