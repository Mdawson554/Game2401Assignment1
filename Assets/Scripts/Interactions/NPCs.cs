using System.Collections.Generic;
using Core;
using EventSystem;
using Gameplay;
using Interactions;
using Story;
using UnityEngine;
public class NPCs : MonoBehaviour, IInteractable
{
    public DialogueSO Dialogue;
    public NPCType npcType;
    public bool HasRequirement;
        
    [Header("Requirements")]
    public List<StoryMarker> RequiredMarkers;
    [Header("Unlocks")]
    public List<StoryMarker> MarkersUnlocked;
    
    private void Start()
    {
        EventManager.instance.Subscribe<StoryMarkerUnlockedEvent>(UnlockMarker);
    }
    
    private void OnDestroy()
    {
        if (EventManager.instance != null)
        {
            EventManager.instance.Unsubscribe<StoryMarkerUnlockedEvent>(UnlockMarker);
            EventManager.instance.Unsubscribe<DialogueFinishedEvent>(OnDialogueFinished);
        }
    }
    
    private void OnInteracted()
    {
        if (HasRequirement)
        {
            if (RequiredMarkers.Count > 0)
            {
                Debug.Log($"Requirements unfulfilled for {gameObject.name}. Missing {RequiredMarkers.Count} marker(s)");
                return;
            }
        }
        EventManager.instance.Subscribe<DialogueFinishedEvent>(OnDialogueFinished);
        DeliverDialogue();
    }
    
    public void OnInteract()
    {
        OnInteracted();
    }
    
    public void OnHoverIn()
    {
            
    }
    
    public void OnHoverOff()
    {
            
    }

    private void UnlockMarker(StoryMarkerUnlockedEvent e)
    {
        if (RequiredMarkers.Contains(e.Marker))
        {
            RequiredMarkers.Remove(e.Marker);
            Debug.Log($"NPC '{gameObject.name}' now has marker '{e.Marker.name}'. Remaining requirements: {RequiredMarkers.Count}");
            if (RequiredMarkers.Count == 0)
            {
                Debug.Log($"NPC '{gameObject.name}' is now unlocked!");
            }
        }
    }
    
    public void OnDialogueFinished(DialogueFinishedEvent e)
    {
        EventManager.instance.Unsubscribe<DialogueFinishedEvent>(OnDialogueFinished);
        
        if (EventManager.instance != null)
        {
            foreach (var marker in MarkersUnlocked)
            {
                EventManager.instance.Publish(new StoryMarkerUnlockedEvent(marker));
                Debug.Log($"NPC '{gameObject.name}' unlocked marker '{marker.name}'");
            }
        }
    }
    
    private void DeliverDialogue()
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
}