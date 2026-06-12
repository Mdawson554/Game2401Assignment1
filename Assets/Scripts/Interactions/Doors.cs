using Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Interactions
{
    public class Doors : InteractableObjects
    {
        [SerializeField] private GameObject key;
        
        protected override void OnInteracted()
        {
            if (InventoryManager.Instance.EquippedItem == key)
            {
                OpenDoor();   
            }
            else
            {
                Debug.Log("You can't do that!");
            }
        }
        
        private void OpenDoor()
        {
            Debug.Log("Opening door");
        }

    }
}
