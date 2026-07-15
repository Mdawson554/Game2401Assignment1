using System;
using System.Collections.Generic;
using EventSystem;
using Interactions;
using Interactions.Pickups;
using UnityEngine;

namespace Core
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Clues")] 
        public int TotalClues;
        public int _clueCount = 0;

        public static InventoryManager Instance;
        public GameObject EquippedItem;
        
        public Dictionary<CollectibleTypes, InventoryUIItem> _items = new Dictionary<CollectibleTypes, InventoryUIItem>();
        

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

        private void AddToInventory(PickupEvent obj)
        {
            var type = obj.assignedCollectibleType;
            var uIItem = obj.assignedinventoryUIItem;
            
            if ( _items.TryAdd(type, uIItem))
            {
                InventoryUIManager.Instance.CreateInventoryItem(uIItem);
            }
        }

        public void IncrementClueCount()
        {
            _clueCount++;
            CheckClueAmount();
        }

        private void CheckClueAmount()
        {
            if (_clueCount == TotalClues)
            {
                GameManager.Instance.OnAllCluesCollected();
            }
        }
    }
}

