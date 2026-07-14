using UnityEngine;

namespace States.StateTypes
{
    public class WalkingState : IStates
    {
        private PlayerController playerController;
        public WalkingState(PlayerController player)
        {
            playerController = player;
        }

        public void EnterState()
        {
            playerController.ToggleMovement(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        public void ExecuteState()
        {
            
        }
        public void ExitState()
        {
            
        }
    }
}
