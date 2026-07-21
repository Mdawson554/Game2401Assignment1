using System.Collections.Generic;
using Core;
using EventSystem;
using UnityEngine;

namespace Story
{
    public abstract class StoryInteractable : MonoBehaviour
    {
        [Header("Requirements")]
        public List<StoryRequirement> Requirements;

        [Header("Unlocks")]
        public List<StoryMarker> UnlockMarkers;

        public void Interact()
        {
            foreach (var requirement in Requirements)
            {
                /*if (!StoryManager.Instance.HasMarker(requirement.Marker))
                {
                    UIManager.Instance.DisplayToast(requirement.LockedMonologue);
                    return;
                }*/
            }

            OnSuccessfulInteraction();

            foreach (var marker in UnlockMarkers)
            {
                EventManager.instance.Publish(new StoryMarkerUnlockedEvent(marker));
            }
        }

        protected abstract void OnSuccessfulInteraction();
    }
}