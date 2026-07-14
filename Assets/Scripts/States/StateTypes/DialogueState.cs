using UnityEngine;

namespace States.StateTypes
{
    public class DialogueState : IStates
    {private PlayerStateMachine playerStateMachine;
        public DialogueState(PlayerStateMachine stateMachine)
        {
            playerStateMachine = stateMachine;
        }

        public void EnterState()
        {
            playerStateMachine.playerController.ToggleMovement(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ExecuteState()
        {
            
        }
        public void ExitState()
        {
           
        }
    }
}
