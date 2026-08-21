using _Project.Scripts.Tools;
using UnityEngine;

namespace _Project.Scripts.States
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
            DevLogger.Log("entering state");
        }
        void ExecuteState()
        {
            DevLogger.Log("in state");
        }
        void ExitState()
        {
            DevLogger.Log("exiting state");
        }
        
        
    }
}
