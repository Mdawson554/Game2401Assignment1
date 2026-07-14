using UnityEngine;

namespace States.StateTypes
{
    public class IdleState : IStates
    {
        /// <summary>
        /// a transitional state that enables or disables input 
        /// </summary>
        private PlayerController playerController;
        public IdleState(PlayerController player)
        {
            playerController = player;
        }
        
        public void EnterState()
        {
            /*playerController.ToggleMovement(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;*/
        }
        
        public void ExecuteState()
        {
            
        }
        
        
        public void ExitState()
        {
            
        }
    }
}
