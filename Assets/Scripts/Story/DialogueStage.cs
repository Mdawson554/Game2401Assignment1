using System;
using System.Collections.Generic;
using EventSystem;
using UnityEngine;

namespace Story
{
    [Serializable]
    public class DialogueStage
    {
        [Header("Locked Dialogue (DialogueSO)")]
        public DialogueSO LockedDialogue;
        
        [Header("Unlocked Dialogue (DialogueSO)")]
        public DialogueSO UnlockedDialogue;
        
        [Header("Markers unlocked AFTER this stage finishes")]
        public List<StoryMarker> MarkersUnlocked = new List<StoryMarker>();

        private void UnlockMarker(StoryMarkerUnlockedEvent e)
        {
            MarkersUnlocked.Add(e.Marker);
        }

        public void Init()
        {
            EventManager.instance.Subscribe<StoryMarkerUnlockedEvent>(UnlockMarker);
        }
        public bool IsUnlocked()
        {
            if (!UnlockedDialogue.hasrequirements)
                return true;
            foreach (var req in UnlockedDialogue.requirements)
            {
                if (MarkersUnlocked.Contains(req))
                    return false;
            }
            return true;
        }
        public bool HasRequirements => UnlockedDialogue.hasrequirements;
    }
}