using Interactions;
using UnityEngine;

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
