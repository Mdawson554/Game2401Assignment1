using System.Collections.Generic;
using Core;
using EventSystem;
using Gameplay;
using States;
using Story;
using UnityEngine;

namespace Interactions
{
    public class NPCs : MonoBehaviour, IInteractable
    {
        public DialogueSO Dialogue;
        public NPCType npcType;
        
        [Header("Requirements")]
        public List<StoryRequirement> Requirements;

        [Header("Unlocks")]
        public List<StoryMarker> UnlockMarkers;

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
            
            EventManager.instance.Subscribe<DialogueFinishedEvent>(OnDialogueFinished);
        }
        
        private void OnDestroy()
        {
            if (EventManager.instance != null)
            {
                EventManager.instance.Unsubscribe<StoryMarkerUnlockedEvent>(UnlockMarker);
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
        
        void UnlockMarker(StoryMarkerUnlockedEvent e)
        {
            UnlockMarkers.Add(e.Marker);
            Debug.Log($"Unlocked Story Marker : {e.Marker.name}");
        }

        public bool HasMarker(StoryMarker marker)
        {
            return UnlockMarkers.Contains(marker);
        }
        
        private void Start()
        {
            foreach (var requirement in Requirements)
            {
                EventManager.instance.Subscribe<StoryMarkerUnlockedEvent>(UnlockMarker);
            }
        }

        public void OnDialogueFinished(DialogueFinishedEvent e)
        {
            if (EventManager.instance != null)
            {
                EventManager.instance.Unsubscribe<DialogueFinishedEvent>(OnDialogueFinished);
            }
        }
    }
}
