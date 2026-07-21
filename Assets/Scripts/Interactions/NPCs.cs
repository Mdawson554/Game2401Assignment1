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

    private void OnInteracted()
    {
        EventManager.instance.Subscribe<DialogueFinishedEvent>(OnDialogueFinished);
        if (HasRequirement)
        {
            if (RequiredMarkers.Count <=0)
            {
                DeliverDialogue();
            }
            else
            {
                Debug.Log("requirments unfufilled");
                return;
            }
        }
        DeliverDialogue();
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
        EventManager.instance.Unsubscribe<DialogueFinishedEvent>(OnDialogueFinished);
        if (RequiredMarkers.Contains(e.Marker))
        {
            
            EventManager.instance.Unsubscribe<StoryMarkerUnlockedEvent>(UnlockMarker);
            RequiredMarkers.Remove(e.Marker);
        }
        Debug.Log($"Unlocked Story Marker : {e.Marker.name}");
    }
        
    private void Start()
    {
        
    }

    public void OnDialogueFinished(DialogueFinishedEvent e)
    {
        if (EventManager.instance != null)
        {
            foreach (var Marker in MarkersUnlocked)
            {
                EventManager.instance.Publish(new StoryMarkerUnlockedEvent(Marker));
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