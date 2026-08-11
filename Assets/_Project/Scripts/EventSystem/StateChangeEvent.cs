using States;
using Unity.VisualScripting;
using UnityEngine;

namespace EventSystem
{
    public class StateChangeEvent : IEvent
    {
        public IStates assignedState;
    
        public StateChangeEvent(IStates state)
        {
            assignedState = state;
        }
    }
}