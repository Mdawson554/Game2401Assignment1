using _Project.Scripts.Core;
using UnityEngine;

namespace _Project.Scripts.States.StateTypes
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
        public void Update()
        {
            
        }
    }
}
