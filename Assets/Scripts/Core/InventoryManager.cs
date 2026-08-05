using System.Collections.Generic;
using EventSystem;
using Interactions;
using UnityEngine;
namespace Core
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("Clues")]
        [SerializeField] private int totalClues;
        private int _clueCount = 0;
        private GameObject _equippedItem;
        public int TotalClues => totalClues;
        public int ClueCount => _clueCount;
        public GameObject EquippedItem
        {
            get => _equippedItem;
            set => _equippedItem = value;
        }
        private Dictionary<CollectibleTypes, InventoryUIItem> items = new Dictionary<CollectibleTypes, InventoryUIItem>();
        private Dictionary<CollectibleTypes, BaseItem>  BaseItems = new Dictionary<CollectibleTypes, BaseItem>();
        public Dictionary<BaseItem, int> Keys { get; private set; } = new Dictionary<BaseItem, int>();
        
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
            Debug.Log("added key");
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
    
            if (items.TryAdd(itemType, uIItem))
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
            _clueCount++;
            CheckClueAmount();
        }
        
        private void CheckClueAmount()
        {
            if (_clueCount == totalClues)
            {
                GameManager.Instance.OnAllCluesCollected();
            }
        }
    }
}