using _Project.Scripts.Gameplay;
using UnityEngine;

namespace _Project.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/DialogueSO")]
    public class DialogueSO : ScriptableObject
    {
        public DialogueStruct[] DialogueArray;

        public DialogueType assignedType;
    }

    public enum DialogueType
    {
        RandomDialogue,
        SequentialDialogue,
        ItemDialogue,
    }
}