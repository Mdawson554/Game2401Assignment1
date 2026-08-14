using UnityEngine;

namespace _Project.Scripts.States.StateTypes
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
            Debug.Log("idle");
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