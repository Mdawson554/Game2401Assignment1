using System.Collections.Generic;
using Gameplay;
using Story;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public DialogueStruct []DialogueArray;
    public DialogueStruct LockedDialogue;
    public DialogueType assignedType;
}

public enum DialogueType
{
    RandomDialogue,
    SequentialDialogue,
    ItemDialogue,
}


