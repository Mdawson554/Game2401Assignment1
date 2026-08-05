using System.Collections.Generic;
using Gameplay;
using Story;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public DialogueStruct []DialogueArray;
    public DialogueType assignedType;
    public bool hasrequirements => requirements.Count >= 1;
    public List<StoryMarker> requirements = new List<StoryMarker>();
}

public enum DialogueType
{
    RandomDialogue,
    SequentialDialogue,
    ItemDialogue,
}


