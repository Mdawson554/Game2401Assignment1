using Gameplay;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public DialogueStruct []DialogueArray;
    public DialogueType assignedType;


}

public enum DialogueType
{
    RandomDialogue,
    SequentialDialogue,
    ItemDialogue,
}
