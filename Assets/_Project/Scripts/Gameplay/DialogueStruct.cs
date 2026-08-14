using System;
using _Project.Scripts.Story;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    [Serializable]
    public class DialogueStruct
    {
        public string Dialogue;
        public NPCType Type;
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