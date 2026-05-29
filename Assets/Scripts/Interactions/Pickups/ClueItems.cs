using UnityEngine;

namespace Interactions.Pickups
{
    public class ClueItems : InteractableObjects //,ICollectible
    {
        protected override void OnInteracted()
        {
            //logic for what happens when I pick up a key
            //signal to UI manager and audio manager
            //maybe make an event
        }

        protected override void OnFullfiledRequirements()
        {
            //logic for when I've used my key and stuff
            //probably indicate to the player that they key is only used for the one room.
            //signal to UI manager and audio manager
            //maybe make an event
        }
    }
}
