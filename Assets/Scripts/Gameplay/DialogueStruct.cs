using System;
using System.Collections.Generic;
using Story;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public struct DialogueStruct
    {
        public string Dialogue;
        public NPCType Type;
        public Sprite DialogueSprite;
        public TMP_Text DialogueText;
        public Font DialogueFont;
        public Color DialogueColor;

        public bool hasrequirements;
        public bool requirementFufilled;
        public StoryMarker StoryMarkerRequirement;
    }

    public enum NPCType
    {
        EssentialNPC,
        
        CommonNPC,
        
        DrunkNPC,
        
        BouncerNPC,
    }
    
}
