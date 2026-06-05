using Core;
using UnityEngine;

namespace Interactions.Pickups
{
    public class Keys : InteractableObjects,ICollectible
    {
        protected override void OnInteracted()
        {
            Debug.Log("Keys Interacted");
        }

        protected override void OnFullfiledRequirements()
        {
            InventoryManager.Instance.UnequipItem();
            Debug.Log("Key used and unequipped");
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
