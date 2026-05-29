using System;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public struct DialogueStruct
    {
        public string Dialogue;
        public DialogueType Type;
        public Sprite DialogueSprite;
        public TMP_Text DialogueText;
        public Font DialogueFont;
        public Color DialogueColor;
    }

    public enum DialogueType
    {
        EssentialNPC,
        
        CommonNPC,
        
        DrunkNPC,
        
        Hints,
        
        Clues,
        
        Monologue,
    }
}
