using System.Collections.Generic;
using Interactions;
using UnityEngine;

namespace Core
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        
        private Dictionary<GameObject, string> inventoryDictionary = new Dictionary<GameObject, string>();
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
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
