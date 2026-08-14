using _Project.Scripts.States;

namespace _Project.Scripts.EventSystem
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