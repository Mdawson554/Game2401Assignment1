using Core;
using EventSystem;
using Gameplay;
using States;
using UnityEngine;

namespace Interactions
{
    public class NPCs : InteractableObjects
    {
        public DialogueSO Dialogue;
        public DialogueType dialogueType;

        protected override void OnInteracted()
        { 
            EventManager.instance.Publish(new StateChangeEvent(GameManager.Instance.playerStateMachine.dialoguestate));
            switch (dialogueType)   
            {
                case DialogueType.CommonNPC:
                    DialogueManager.Instance.SetRandomDialogue(Dialogue);
                    break;
                case DialogueType.DrunkNPC:
                    DialogueManager.Instance.SetRandomDialogue(Dialogue);
                    break;
                case DialogueType.BouncerNPC:
                    DialogueManager.Instance.SetRandomDialogue(Dialogue);
                    break;
                case DialogueType.EssentialNPC:
                    DialogueManager.Instance.SetSequentialDialogue(Dialogue, InteractableObjectTypes.NPC);
                    break;
            }
        }
    }
}
