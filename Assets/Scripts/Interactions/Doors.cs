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
                Debug.logger.Log("nuh uh");
            }
        }
        
        private void OpenDoor()
        {
            Debug.logger.Log("Opening door");
        }

    }
}
