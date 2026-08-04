using System;
using System.Collections.Generic;
using UnityEngine;

namespace Story
{
    [Serializable]
    public class DialogueStage
    {
        [Header("Markers required to unlock this stage")]
        public List<StoryMarker> Requirements = new List<StoryMarker>();
        
        [Header("Locked Dialogue (DialogueSO)")]
        public DialogueSO LockedDialogue;
        
        [Header("Unlocked Dialogue (DialogueSO)")]
        public DialogueSO UnlockedDialogue;
        
        [Header("Markers unlocked AFTER this stage finishes")]
        public List<StoryMarker> MarkersUnlocked = new List<StoryMarker>();
        public bool IsUnlocked(Func<StoryMarker, bool> hasMarker)
        {
            if (Requirements.Count == 0)
                return true;
            foreach (var req in Requirements)
            {
                if (req != null && !hasMarker(req))
                    return false;
            }
            return true;
        }
        public bool HasRequirements => Requirements.Count > 0;

        public bool HasValidLocked =>
            LockedDialogue != null && LockedDialogue.DialogueArray != null &&
            LockedDialogue.DialogueArray.Length > 0;

        public bool HasValidUnlocked =>
            UnlockedDialogue != null && UnlockedDialogue.DialogueArray != null &&
            UnlockedDialogue.DialogueArray.Length > 0;
    }
}