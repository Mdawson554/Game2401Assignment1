using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class BaseDialogue
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
        Npc,
        
        Hints,
        
        Clues,
        
        Monologue,
    }
}
