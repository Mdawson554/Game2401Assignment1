using UnityEngine;

namespace _Project.Scripts.Interactions
{
    public class BaseItem : MonoBehaviour
    {
        public CollectibleTypes assignedCollectibleType;
        public InventoryUIItem assignedinventoryUIItem;
        public int KeyValue;
    
        public virtual string GetItemName()
        {
            return "Item";
        }
    }
}
