using UnityEngine;

namespace States.StateTypes
{
    public class PauseState : IStates
    {
        private PlayerController playerController;
        private IStates statesImplementation;

        public PauseState(PlayerController player)
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
