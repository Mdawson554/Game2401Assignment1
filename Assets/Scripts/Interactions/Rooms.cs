using UnityEngine;

namespace Interactions
{
    public class Rooms : InteractableObjects
    {
        protected override void OnInteracted()
        {
            //check for the required key
            //logic for what happens when I open the door---trigger an animation
            //signal to UI manager and audio manager
            //maybe make an event
        }

        protected override void OnFullfiledRequirements()
        {
            //signal to UI manager and audio manager
        }
    }
}
