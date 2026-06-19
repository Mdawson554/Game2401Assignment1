using System;
using System.Collections.Generic;
using Interactions;
using Interactions.Pickups;
using UnityEngine;

namespace Core
{
    public class InventoryManager : MonoBehaviour
    {
         [SerializeField] private int TotalClues;
        
        public static InventoryManager Instance;
        private int _clueCount = 0;
        
        private Dictionary<InteractableObjects,InteractableObjectTypes> _interactableObjects = new Dictionary<InteractableObjects, InteractableObjectTypes>();
        public GameObject EquippedItem;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
            
        }

        public void AddItemToInventory(InteractableObjects interactableObject, InteractableObjectTypes type)
        {
            if (interactableObject != null)
            {
                _interactableObjects.TryAdd(interactableObject, type);
                
                if (_interactableObjects.ContainsKey(interactableObject))
                {
                    interactableObject.count++;
                }
            }
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
