using UnityEngine;

namespace _Project.Scripts.States.StateTypes
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
