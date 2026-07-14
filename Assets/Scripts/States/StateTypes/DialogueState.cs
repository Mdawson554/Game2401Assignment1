using UnityEngine;

namespace States.StateTypes
{
    public class DialogueState : IStates
    {
        private PlayerController playerController;
        public DialogueState(PlayerController player)
        {
            playerController = player;
        }

        public void EnterState()
        {
            playerController.ToggleMovement(false);
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
