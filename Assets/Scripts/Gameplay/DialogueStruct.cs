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

        public StoryMarker RequiredMarkers;
        public StoryMarker ProducedMarkers;

        public string LockedDialogue;
        public Color LockedDialogueColor;

        public bool HasRequirements =>
            RequiredMarkers != null;
    }

    public enum NPCType
    {
        EssentialNPC,
        CommonNPC,
        DrunkNPC,
        BouncerNPC,
    }
}