using UnityEngine;

namespace Interactions
{
    /// <summary>
    /// Classs that covers all interactable objects such as NPCs , items/Clues, keys/puzzle items  and rooms
    /// </summary>
    public class InteractableObjects : MonoBehaviour, IInteractable 
    {
        protected virtual void OnInteracted()
        {
            //hey you turned me on and gav me attention? 
        }

        protected virtual void OnFullfiledRequirements()
        {
            //I did everything for you
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
}
