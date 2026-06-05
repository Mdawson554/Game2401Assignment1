using System.Collections.Generic;
using Interactions;
using UnityEngine;

namespace Core
{
    public class InventoryManager : MonoBehaviour
    {
         [SerializeField] private int TotalClues;
        
        public static InventoryManager Instance;
        private int _clueCount = 0;
        
        private Dictionary<InteractableObjects, string> inventoryDictionary;
        
        public GameObject EquippedItem;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
            inventoryDictionary = new Dictionary<InteractableObjects, string>();
        }

        public void AddItemToInventory(InteractableObjects interactableObject, string Item)
        {
            if (interactableObject != null)
            {
                inventoryDictionary.TryAdd(interactableObject, Item);
                //instantiate inventory ui element matching item type
                
                if (inventoryDictionary.ContainsKey(interactableObject))
                {
                    switch (interactableObject.interactableObjectType)
                    {
                        case InteractableObjectTypes.Clues:
                            IncrementClueCount();
                            break;
                        case InteractableObjectTypes.StandardItem:
                            break;
                    }
                }
            }
        }
        
        public void EquipItem(GameObject item)
        {
            EquippedItem = item;
        }

        public void UnequipItem()
        {
            EquippedItem = null;
        }
        
        private void IncrementClueCount()
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
