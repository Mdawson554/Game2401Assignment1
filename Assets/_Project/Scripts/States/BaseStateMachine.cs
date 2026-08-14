using UnityEngine;

namespace _Project.Scripts.States
{
    public class BaseStateMachine : MonoBehaviour
    {
        public IStates currentState;

        public virtual void changeState(IStates newState)
        {
            if (newState == currentState)
            {
                return;
            }
            currentState?.ExitState();
            currentState = newState;
            currentState?.EnterState();
        }
    
        public virtual void Update()
        {
            currentState?.Update();
        }

        public virtual void FixedUpdate()
        {
            currentState?.FixedUpdate();
        }
    }
}
