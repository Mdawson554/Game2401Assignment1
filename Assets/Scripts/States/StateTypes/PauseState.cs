using Core;
using UnityEngine;

namespace States.StateTypes
{
    public class PauseState : IStates
    {
        private PlayerStateMachine playerStateMachine;
        public PauseState(PlayerStateMachine stateMachine)
        {
            playerStateMachine = stateMachine;
        }

        public void EnterState()
        {
            GameManager.Instance.Pause();
            Debug.Log("Pause");
            playerStateMachine.inputManager.EnableMoveInput(false);
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
