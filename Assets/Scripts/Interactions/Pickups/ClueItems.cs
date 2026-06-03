using Core;
using Gameplay;
using UnityEngine;

namespace Interactions.Pickups
{
    public class ClueItems : InteractableObjects,ICollectible
    {
        [SerializeField] private DialogueSO _cluedialogueSO;
        [SerializeField] private string itemName;

        protected override void OnInteracted()
        {
            DialogueManager.Instance.SetSequentialDialogue(_cluedialogueSO, InteractableObjectTypes.Item);
            
            if (itemName != null)
            {
                InventoryManager.Instance.AddItemToInventory(this.gameObject, itemName);
                Debug.Log("dialogue from item"); 
            } 
        }
        
        protected override void OnFullfiledRequirements()
        {
            //logic for when I've used my key and stuff
            //probably indicate to the player that they key is only used for the one room.
            //signal to UI manager and audio manager
            //maybe make an event
            Debug.Log("clues FullfiledRequirements");
        }
        
        public void OnCollected()
        {
            // TODO: DOTween 
            Debug.Log("ClueItem collected tween placeholder");
        }
        
        public void OnCollectEffect()
        {
            // TODO: ParticleSystem
            Debug.Log("ClueItem particle effect placeholder");
        }
    }
}
