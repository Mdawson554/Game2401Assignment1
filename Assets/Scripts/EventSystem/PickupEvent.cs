using Interactions;
using States;
using Unity.VisualScripting;
using UnityEngine;

namespace EventSystem
{
    public class PickupEvent : IEvent
    {
        public CollectibleTypes assignedCollectibleType;
        public InventoryUIItem assignedinventoryUIItem;
    
        public PickupEvent(CollectibleTypes collectibles, InventoryUIItem inventoryUIItem)
        {
            assignedCollectibleType = collectibles;
            assignedinventoryUIItem = inventoryUIItem;
        }
    }
}