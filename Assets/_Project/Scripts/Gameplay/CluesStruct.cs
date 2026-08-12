using System;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    [Serializable]
    public struct CluesStruct
    {
        public string Clue;
        public ClueType Type;
        public Sprite ClueSprite;
        public TMP_Text ClueText;
        public Font ClueFont;
        public Color ClueColor;
    }

    public enum ClueType
    {
        instructions,
        
        hints,
    }
}
