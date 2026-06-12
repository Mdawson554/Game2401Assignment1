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
        
        protected virtual void OnInteracted()
        {
            
        }

        protected virtual void OnFullfiledRequirements()
        {
            Debug.Log("object fufilled");
            GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
        }

        public void OnHoverIn()
        {
            Debug.Log("InteractableObjects OnHoverIn");
        }

        public void OnInteract()
        {
            Debug.Log("InteractableObjects Oninteract");
            OnInteracted();
        }

        public void OnHoverOff()
        {
           Debug.Log("InteractableObjects OnHoveroff"); 
        }
    }
    
    public enum InteractableObjectTypes
    {
        Clues,
        Keys,
        StandardItem,
        NPC,
    }
    
    
    
    
    
    
}
