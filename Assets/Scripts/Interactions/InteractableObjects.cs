using Core;
using UnityEngine;

namespace Interactions
{
    /// <summary>
    /// Classs that covers all interactable objects such as NPCs , items/Clues, keys/puzzle items  and rooms
    /// </summary>
    public class InteractableObjects : MonoBehaviour, IInteractable 
    {
        public InteractableObjectTypes interactableObjectType;
        public int count;
        
        protected virtual void OnInteracted()
        {
            
        }
        
        public void OnInteract()
        {
            OnInteracted();
        }
        
        public void OnHoverIn()
        {
            
        }
        
        public void OnHoverOff()
        {
            
        }
        
    }
    
    public enum InteractableObjectTypes
    {
        Clues,
        Keys,
        StandardItem,
        NPC,
        Door,
    }
}

