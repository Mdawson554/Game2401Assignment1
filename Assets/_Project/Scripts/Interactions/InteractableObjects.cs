using Core;
using UnityEngine;

namespace Interactions
{
    /// <summary>
    /// Classs that covers all interactable objects such as NPCs , items/Clues, keys/puzzle items  and rooms
    /// </summary>
    public class InteractableObjects : MonoBehaviour, IInteractable 
    {
        public int count;
        
        protected virtual void OnInteracted()
        {
            
        }
        
        public void OnInteract()
        {
            OnInteracted();
        }
    }
}

