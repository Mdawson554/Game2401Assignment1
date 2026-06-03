using System.Collections.Generic;
using Interactions;
using UnityEngine;

namespace Core
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        
        private Dictionary<GameObject, string> inventoryDictionary;
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
            inventoryDictionary = new Dictionary<GameObject, string>();
        }

        public void AddItemToInventory(GameObject interactableObject, string Item)
        {
            if (interactableObject != null)
            {
                inventoryDictionary.TryAdd(interactableObject, Item);
            }
        }
    }
}
