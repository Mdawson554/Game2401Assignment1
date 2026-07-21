using Core;
using Interactions;
using UnityEngine;

namespace Story
{
    public class EssentialNPC : StoryInteractable, IInteractable
    {
        [SerializeField] private DialogueSO dialogue;

        protected override void OnSuccessfulInteraction()
        {
            DialogueManager.Instance.SetSequentialDialogue(dialogue);
        }

        public void OnInteract()
        {
            Interact();
        }
    }
}