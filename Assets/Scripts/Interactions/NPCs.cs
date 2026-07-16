using Core;
using EventSystem;
using Gameplay;
using States;
using UnityEngine;

namespace Interactions
{
    public class NPCs : MonoBehaviour, IInteractable
    {
        public DialogueSO Dialogue;
        public NPCType npcType;

        private void OnInteracted()
        { 
            EventManager.instance.Publish(new StateChangeEvent(GameManager.Instance.playerStateMachine.dialoguestate));
            switch (npcType)   
            {
                case NPCType.CommonNPC:
                    DialogueManager.Instance.SetRandomDialogue(Dialogue);
                    break;
                case NPCType.DrunkNPC:
                    DialogueManager.Instance.SetRandomDialogue(Dialogue);
                    break;
                case NPCType.BouncerNPC:
                    DialogueManager.Instance.SetRandomDialogue(Dialogue);
                    break;
                case NPCType.EssentialNPC:
                    DialogueManager.Instance.SetSequentialDialogue(Dialogue);
                    break;
            }
        }

        public void OnHoverIn()
        {
            
        }

        public void OnInteract()
        {
            OnInteracted();
        }

        public void OnHoverOff()
        {
            
        }
    }
}
