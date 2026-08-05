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
    public DialogueSO Dialogue;
    public NPCType npcType;
    private bool _isInteracting;
    public List<DialogueStruct> structs = new List<DialogueStruct>();
    
    private void Start()
    {
        EventManager.instance.Subscribe<StoryMarkerUnlockedEvent>(OnMarkerUnlocked);
    }
    // bellow in marker unlocked event
    //npc must listen for story marker events then loop through the array of dialog structs in the assigned public Dialogue variable
    //for each dialog struct in array check if it has a requirement, if has requirement check if assigned requirement matches that of the event 
    // if it matches set the requirement met bool in the specific dialogue struct in the array.

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

    public void Init()
    {
        foreach (var dialogueStruct in Dialogue.DialogueArray)
        {
            DialogueStruct stru = new DialogueStruct();
            stru = dialogueStruct;
            structs.Add(stru);
        }
    }

    private void OnMarkerUnlocked(StoryMarkerUnlockedEvent e)
    {
        for(int i = 0; i <= structs.Count; i++)
        {
            if (structs[i].hasrequirements)
            {
                if (e.Marker == structs[i].StoryMarkerRequirement)
                {
                    //structs[i].requirementFufilled = true;
                }
            }
        }
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