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
            DialogueManager.Instance.currentdialogueSO = Dialogue;
            switch (dialogueType)
            {
                case DialogueType.CommonNPC:
                    DialogueManager.Instance.setstandardNPCDialogue();
                    Debug.Log("dialogue with common NPC");
                    break;
                case DialogueType.DrunkNPC:
                    DialogueManager.Instance.setstandardNPCDialogue();
                    Debug.Log("dialogue with drunk NPC");
                    break;
                case DialogueType.EssentialNPC:
                    DialogueManager.Instance.setessentialNPCDialogue();
                    Debug.Log("dialogue with essential NPC");
                    break;
            }
        }
    }
}
