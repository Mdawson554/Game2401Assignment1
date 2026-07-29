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
    public string LockedMessage = "They don't want to talk to me right now...";
    private bool _isInteracting = false;
    
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
        {
            Debug.LogWarning($"NPC '{gameObject.name}' is already in an interaction. Ignoring input.");
            return;
        }
        
        if (HasRequirement)
        {
            if (RequiredMarkers.Count > 0)
            {
                Debug.Log($"Requirements unfulfilled for {gameObject.name} ({RequiredMarkers.Count}). Missing {RequiredMarkers.Count} marker(s)");
                UIManager.Instance.DisplayToast(LockedMessage);
                return;
            }
        }
        _isInteracting = true;
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
        if (e == null || e.Marker == null)
        {
            Debug.LogError($"NPC '{gameObject.name}': Received invalid StoryMarkerUnlockedEvent");
            return;
        }
        
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
        if (e == null)
        {
            Debug.LogError($"NPC '{gameObject.name}': Received null DialogueFinishedEvent");
            return;
        }
        EventManager.instance.Unsubscribe<DialogueFinishedEvent>(OnDialogueFinished);
        if (EventManager.instance != null)
        {
            foreach (var marker in MarkersUnlocked)
            {
                if (marker != null)
                {
                    EventManager.instance.Publish(new StoryMarkerUnlockedEvent(marker));
                    Debug.Log($"NPC '{gameObject.name}' unlocked marker '{marker.name}'");
                }
                else
                {
                    Debug.LogError($"NPC '{gameObject.name}': MarkersUnlocked contains null marker!");
                }
            }
        }
        else
        {
            Debug.LogError($"NPC '{gameObject.name}': EventManager.instance is null during OnDialogueFinished");
        }
        _isInteracting = false;
    }
    
    private void DeliverDialogue()
    {
        if (DialogueManager.Instance == null)
        {
            Debug.LogError($"NPC '{gameObject.name}': DialogueManager.Instance is null");
            _isInteracting = false;
            return;
        }
        if (GameManager.Instance == null || GameManager.Instance.playerStateMachine == null)
        {
            Debug.LogError($"NPC '{gameObject.name}': GameManager or playerStateMachine is null");
            _isInteracting = false;
            return;
        }
        if (Dialogue == null)
        {
            Debug.LogError($"NPC '{gameObject.name}': Dialogue ScriptableObject is not assigned!");
            _isInteracting = false;
            return;
        }
        
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
            default:
                Debug.LogError($"NPC '{gameObject.name}': Unknown NPCType {npcType}");
                _isInteracting = false;
                break;
        }
    }
}