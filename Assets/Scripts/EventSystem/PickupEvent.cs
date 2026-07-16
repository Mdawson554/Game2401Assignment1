using Interactions;
using States;
using Unity.VisualScripting;
using UnityEngine;

namespace EventSystem
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