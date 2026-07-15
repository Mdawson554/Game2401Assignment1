using UnityEngine;

namespace States.StateTypes
{
    public class WalkingState : IStates
    {
        private PlayerStateMachine playerStateMachine;
        public WalkingState(PlayerStateMachine stateMachine)
        {
            playerStateMachine = stateMachine;
        }

        public void EnterState()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        public void ExecuteState()
        {
            
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
