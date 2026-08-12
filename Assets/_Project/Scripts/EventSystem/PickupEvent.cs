using _Project.Scripts.Interactions;

namespace _Project.Scripts.EventSystem
{
    public class PickupEvent : IEvent
    {
        
        public BaseItem assignedItem;
    
        public PickupEvent(BaseItem baseItem)
        {
            assignedItem = baseItem;
        }
    }
}