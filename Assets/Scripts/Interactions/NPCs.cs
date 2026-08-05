using System;
using System.Collections.Generic;
using Core;
using EventSystem;
using Gameplay;
using Interactions;
using Story;
using UnityEngine;

public class NPCs : MonoBehaviour, IInteractable
{
    [Header("Fallback Dialogue (optional)")]
    public DialogueSO Dialogue;

    [Header("Progressive Dialogue Stages")]
    public List<DialogueStage> stages = new List<DialogueStage>();

    public NPCType npcType;

    private bool _isInteracting;
    private DialogueStage _activeStage;

    private void Start()
    {
        EventManager.instance.Subscribe<StoryMarkerUnlockedEvent>(OnMarkerUnlocked);
        foreach (var stage in stages)
        {
            if (stage == null) continue;
            stage.Init();
        }
    }

    private void OnDestroy()
    {
        if (EventManager.instance != null)
        {
            EventManager.instance.Unsubscribe<StoryMarkerUnlockedEvent>(OnMarkerUnlocked);
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

    private void OnMarkerUnlocked(StoryMarkerUnlockedEvent e)
    {
        // StoryManager owns progression
    }

    public void OnDialogueFinished(DialogueFinishedEvent e)
    {
        EventManager.instance.Unsubscribe<DialogueFinishedEvent>(OnDialogueFinished);

        if (_activeStage != null && _activeStage.MarkersUnlocked != null)
        {
            foreach (var marker in _activeStage.MarkersUnlocked)
            {
                if (marker != null && !StoryManager.Instance.HasMarker(marker))
                {
                    EventManager.instance.Publish(new StoryMarkerUnlockedEvent(marker));
                }
            }
        }

        _activeStage = null;
        _isInteracting = false;
    }

    private bool HasMarker(StoryMarker marker)
    {
        return StoryManager.Instance.HasMarker(marker);
    }

    private bool IsStageCompleted(DialogueStage stage)
    {
        if (stage.MarkersUnlocked == null || stage.MarkersUnlocked.Count == 0)
            return false;

        foreach (var marker in stage.MarkersUnlocked)
        {
            if (marker != null && !HasMarker(marker))
                return false;
        }

        return true;
    }

    private DialogueStage GetCurrentStage()
    {
        DialogueStage lastUnlocked = null;

        foreach (var stage in stages)
        {
            if(stage == null) continue;
            bool requirementsMet = stage.IsUnlocked();
            bool isLocked = stage.HasRequirements && !requirementsMet;

            // If stage is locked, return it (player sees locked dialogue)
            if (isLocked)
            {
                return stage;
            }

            // If stage is unlocked but not completed, return it
            if (!IsStageCompleted(stage))
            {
                return stage;
            }

            // Stage is completed, track it and continue
            lastUnlocked = stage;
        }
        
        return lastUnlocked;
    }

    private void DeliverDialogue()
    {
        var stage = GetCurrentStage();

        if (stage == null)
        {
            _activeStage = null;

            if (Dialogue != null)
            {
                PlayDialogueSO(Dialogue);
            }
            else
            {
                _isInteracting = false;
            }
            return;
        }

        bool requirementsMet = stage.IsUnlocked();
        bool isLocked = stage.HasRequirements && !requirementsMet;

        if (isLocked)
        {
            _activeStage = stage;

            if (stage.LockedDialogue)
            {
                PlayDialogueSO(stage.LockedDialogue);
            }
            else
            {
                _isInteracting = false;
            }
            return;
        }

        _activeStage = stage;

        if (stage.UnlockedDialogue)
        {
            PlayDialogueSO(stage.UnlockedDialogue);
        }
        else if (stage.LockedDialogue)
        {
            PlayDialogueSO(stage.LockedDialogue);
        }
        else if (Dialogue != null)
        {
            PlayDialogueSO(Dialogue);
        }
        else
        {
            _isInteracting = false;
        }
    }

    private void PlayDialogueSO(DialogueSO dialogueSo)
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
                DialogueManager.Instance.SetRandomDialogue(dialogueSo);
                GameManager.Instance.playerStateMachine.changeState(
                    GameManager.Instance.playerStateMachine.idlestate);
                _isInteracting = false;
                break;

            case NPCType.EssentialNPC:
                DialogueManager.Instance.SetSequentialDialogue(dialogueSo);
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