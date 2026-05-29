using UnityEngine;

namespace Interactions
{
    /// <summary>
    /// Classs that covers all interactable objects such as NPCs , items/Clues, keys/puzzle items  and rooms
    /// </summary>
    public class InteractableObjects : MonoBehaviour 
    {
        protected virtual void OnInteracted()
        {
            //hey you turned me on and gav me attention? 
        }

        protected virtual void OnFullfiledRequirements()
        {
            //I did everything for you
        }
    }
}
