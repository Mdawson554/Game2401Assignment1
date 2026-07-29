using UnityEngine;

namespace States.StateTypes
{
    public class IdleState : IStates
    {
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
            playerStateMachine.playerController.CalculateMouseAndCam();
        }

        public void FixedUpdate()
        {
            playerStateMachine.playerController.PlayerMovement();
        }
    }
}