using UnityEngine;

namespace States.StateTypes
{
    public class DialogueState : IStates
    {
        private PlayerStateMachine playerStateMachine;

        public DialogueState(PlayerStateMachine stateMachine)
        {
            playerStateMachine = stateMachine;
        }

        public void EnterState()
        {
            // Disable movement while talking
            playerStateMachine.inputManager.EnableMoveInput(false);

            // Keep mouse hidden so the player can still look around
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ExecuteState()
        {

        }

        public void ExitState()
        {
            // Restore movement once dialogue ends
            playerStateMachine.inputManager.EnableMoveInput(true);
        }

        public void Update()
        {
            // Allow the player to keep looking around
            playerStateMachine.playerController.CalculateMouseAndCam();
        }
    }
}
