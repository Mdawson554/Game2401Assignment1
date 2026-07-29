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
    
    [Header("Feedback")]
    [TextArea(2, 3)]
    
    [SerializeField] public string LockedMessage;
    private bool _isInteracting;
    
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
        if (_isInteracting)
            return;
        if (HasRequirement && RequiredMarkers.Count > 0)
        {
            UIManager.Instance.DisplayToast(LockedMessage);
            return;
        }
        _isInteracting = true;
        if (npcType == NPCType.EssentialNPC)
        {
            EventManager.instance.Subscribe<DialogueFinishedEvent>(OnDialogueFinished);
        }

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
        }
    }
   
    public void OnDialogueFinished(DialogueFinishedEvent e)
    {
        EventManager.instance.Unsubscribe<DialogueFinishedEvent>(OnDialogueFinished);
        if (EventManager.instance != null)
        {
            foreach (var marker in MarkersUnlocked)
            {
                if (marker != null)
                {
                    EventManager.instance.Publish(new StoryMarkerUnlockedEvent(marker));
                }
                
            }
        }
        _isInteracting = false;
    }

    private void setConversation()
    {
        DialogueManager.Instance.SetSequentialDialogue(Dialogue);
        EventManager.instance.Publish(new StateChangeEvent(GameManager.Instance.playerStateMachine.dialoguestate));
    }

    private void setInteraction()
    {
        DialogueManager.Instance.SetRandomDialogue(Dialogue);

        EventManager.instance.Publish(
            new StateChangeEvent(GameManager.Instance.playerStateMachine.idlestate));
        _isInteracting = false;
    }
    
    private void DeliverDialogue()
    {
        if (DialogueManager.Instance == null)
        {
            _isInteracting = false;
            return;
        }
        if (GameManager.Instance == null || GameManager.Instance.playerStateMachine == null)
        {
            _isInteracting = false;
            return;
        }
        if (Dialogue == null)
        {
            _isInteracting = false;
            return;
        }
        
        switch (npcType)
        {
            case NPCType.CommonNPC:
                setInteraction();
                break;
            case NPCType.DrunkNPC:
                setInteraction();
                break;
            case NPCType.BouncerNPC:
                setInteraction();
                break;
            case NPCType.EssentialNPC:
                setConversation();
                break;
            default:
                _isInteracting = false;
                break;
        }
    }
}