using UnityEngine;

namespace _Project.Scripts.States.StateTypes
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
            playerStateMachine.playerController.SyncIdleToCurrentCamera();
            playerStateMachine.playerController.SetSkipNextIdleCameraFrame(true);
            playerStateMachine.inputManager.EnableMoveInput(true);
        }
        

        public void Update()
        {
            playerStateMachine.playerController.CalculateMouseAndCamDialogue();
        }
    }
}