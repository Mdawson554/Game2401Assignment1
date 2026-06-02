using Core;
using Gameplay;
using UnityEngine;

namespace Interactions
{
    public class NPCs : InteractableObjects
    {
        public DialogueSO Dialogue;
        public DialogueType dialogueType;

        protected override void OnInteracted()
        { 
            switch (dialogueType)
            {
                case DialogueType.CommonNPC:
                    DialogueManager.Instance.SetRandomDialogue(Dialogue);
                    Debug.Log("dialogue with common NPC");
                    break;
                case DialogueType.DrunkNPC:
                    DialogueManager.Instance.SetRandomDialogue(Dialogue);
                    Debug.Log("dialogue with drunk NPC");
                    break;
                case DialogueType.EssentialNPC:
                    DialogueManager.Instance.SetSequentialDialogue(Dialogue, InteractableObjectTypes.NPC);
                    Debug.Log("dialogue with essential NPC");
                    break;
            }
        }
    }
}
