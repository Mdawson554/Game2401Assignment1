using _Project.ScriptableObjects;
using _Project.Scripts.Core;
using _Project.Scripts.EventSystem;
using _Project.Scripts.Gameplay;
using UnityEngine;

namespace _Project.Scripts.Interactions
{
    public class NPCs : MonoBehaviour, IInteractable
    {
        public DialogueSO Dialogue;
        public NPCType npcType;
        private bool _isInteracting;
   
        private void OnDestroy()
        {
            if (EventManager.instance != null)
            {
                EventManager.instance.Unsubscribe<DialogueFinishedEvent>(OnDialogueFinished);
            }
        }

        public void OnInteract()
        {
            if (_isInteracting) return;

            _isInteracting = true;

            if (npcType == NPCType.EssentialNPC)
            {
                EventManager.instance.Subscribe<DialogueFinishedEvent>(OnDialogueFinished);
            }

            DeliverDialogue();
        }

        private void OnDialogueFinished(DialogueFinishedEvent e)
        {
            EventManager.instance.Unsubscribe<DialogueFinishedEvent>(OnDialogueFinished);
            _isInteracting = false;
        }

        private void DeliverDialogue()
        {
            PlayDialogueSO();
        }

        private void PlayDialogueSO()
        {
            if (DialogueManager.Instance == null ||
                GameManager.Instance == null ||
                GameManager.Instance.playerStateMachine == null)
            {
                _isInteracting = false;
                return;
            }
            switch (npcType)
            {
                case NPCType.CommonNPC:
                case NPCType.DrunkNPC:
                case NPCType.BouncerNPC:
                    DialogueManager.Instance.SetRandomDialogue(Dialogue);
                    GameManager.Instance.playerStateMachine.changeState(
                        GameManager.Instance.playerStateMachine.idlestate);
                    _isInteracting = false;
                    break;

                case NPCType.EssentialNPC:
                    Debug.Log("DeathToAll");
                    DialogueManager.Instance.SetSequentialDialogue(Dialogue);
                    GameManager.Instance.playerStateMachine.changeState(
                        GameManager.Instance.playerStateMachine.dialoguestate);
                    break;

                default:
                    _isInteracting = false;
                    break;
            }
        }

        public void OnHoverIn() { }
        public void OnHoverOff() { }
    }
}