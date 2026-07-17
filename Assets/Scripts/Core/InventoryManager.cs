using System.Collections.Generic;
using EventSystem;
using Interactions;
using Pickups;
using UnityEngine;
namespace Core
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Clues")] 
        public int TotalClues;
        public int ClueCount = 0;
        public static InventoryManager Instance;
        public GameObject EquippedItem;
        
        public Dictionary<CollectibleTypes, InventoryUIItem> Items = new Dictionary<CollectibleTypes, InventoryUIItem>();
        public Dictionary<CollectibleTypes, BaseItem>  BaseItems = new Dictionary<CollectibleTypes, BaseItem>();
        public Dictionary<BaseItem, int> Keys = new Dictionary<BaseItem, int>();
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }
        
        private void OnEnable()
        {
            EventManager.instance.Subscribe<PickupEvent>(AddToInventory);
        }
        
        private void OnDisable()
        {
            EventManager.instance.Unsubscribe<PickupEvent>(AddToInventory);
        }
        
        private void AddKey(BaseItem item)
        {
            Debug.Log("ahhhhhhh");
            if (item.assignedCollectibleType == CollectibleTypes.Keys)
            {
                Keys.TryAdd(item, item.KeyValue);
            }
        }
        
        private void AddToInventory(PickupEvent obj)
        {
            var item = obj.assignedItem;
            var uIItem = item.assignedinventoryUIItem;
            var itemType = item.assignedCollectibleType;
            AddKey(item);
    
            BaseItems.TryAdd(itemType, item);
    
            if (Items.TryAdd(itemType, uIItem))
            {
                InventoryUIManager.Instance.CreateInventoryItem(uIItem, itemType);
                InventoryUIManager.Instance.IncrementInventoryUIItem(uIItem, itemType, item);  
            }
            else
            {
                InventoryUIManager.Instance.IncrementInventoryUIItem(uIItem, itemType, item);  
            }
        }
        
        public void IncrementClueCount()
        {
            ClueCount++;
            CheckClueAmount();
        }
        
        private void CheckClueAmount()
        {
            if (ClueCount == TotalClues)
            {
                GameManager.Instance.OnAllCluesCollected();
            }
        }
    }
}