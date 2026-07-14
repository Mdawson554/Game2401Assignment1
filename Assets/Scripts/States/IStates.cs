using UnityEngine;

namespace States
{
    public interface IStates
    {
        
        void Update()
        {
            
        }
        
        void FixedUpdate()
        {
            
        }
        
        void EnterState()
        {
            Debug.Log("entering state");
        }
        void ExecuteState()
        {
            Debug.Log("in state");
        }
        void ExitState()
        {
            Debug.Log("exiting state");
        }
        
        
    }
}
