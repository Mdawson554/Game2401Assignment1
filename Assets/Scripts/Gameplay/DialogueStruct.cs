using System;
using System.Collections.Generic;
using Story;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class DialogueStruct
    {
        public string Dialogue;
        public NPCType Type;
        public Sprite DialogueSprite;
        public TMP_Text DialogueText;
        public Font DialogueFont;
        public Color DialogueColor;

        public List<StoryMarker> RequiredMarkers;

        public List<StoryMarker> ProducedMarkers;

        // Simple per-line locked dialogue
        public string LockedDialogue;
        public Color LockedDialogueColor;
        
        public bool HasRequirements =>
            RequiredMarkers != null && RequiredMarkers.Count > 0;
    }

    public enum NPCType
    {
        EssentialNPC,
        CommonNPC,
        DrunkNPC,
        BouncerNPC,
    }
}