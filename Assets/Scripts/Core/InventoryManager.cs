using System;
using System.Collections.Generic;
using Interactions;
using Interactions.Pickups;
using UnityEngine;

namespace Core
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Clues")] 
        [SerializeField] private int TotalClues;
        public int _clueCount = 0; //turn back to private after testing

        public static InventoryManager Instance;

        private Dictionary<InteractableObjects, InteractableObjectTypes> _interactableObjects =
            new Dictionary<InteractableObjects, InteractableObjectTypes>();

        public GameObject EquippedItem;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;

        }

        /*public void AddClueToInventory(InteractableObjects interactableObject, InteractableObjectTypes Type) ////OLD METHOD
        {
            if (interactableObject != null)
            {
                _interactableObjects.TryAdd(interactableObject, Type);

                if (_interactableObjects.ContainsKey(interactableObject))
                {
                    interactableObject.count++;
                    //obj.count++;
                    _clueCount++;
                    Debug.Log("item added");
                    CheckClueAmount();
                }
            }
        }*/
        
        public void AddClueToInventory(InteractableObjects obj, InteractableObjectTypes type)
        {
            if (obj == null) return;

            if (_interactableObjects.TryAdd(obj, type))
            {
                obj.count++;
                _clueCount++;
                CheckClueAmount();
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

