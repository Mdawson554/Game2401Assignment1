using System;
using System.Collections.Generic;
using EventSystem;
using Gameplay;
using Story;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        private int _dialogueIndex;
        private bool _isTransitioning;
        private bool _hasActiveDialogue;
        private DialogueSO _currentDialogueSo;
        
        // MEMORY: This remembers the player's place in the conversation for each DialogueSO
        private Dictionary<DialogueSO, int> _dialogueProgress = new Dictionary<DialogueSO, int>();
        
        public DialogueStruct dialogueStruct;

        private void IncrementDialogue()
        {
            var currentLine = _currentDialogueSo.DialogueArray[_dialogueIndex];

            // If the current line had a requirement that wasn't met, the player is currently 
            // viewing the LockedDialogue. Pressing 'Next' closes it.
            // Notice we DO NOT increment here, so next time they talk, they resume exactly here!
            if (currentLine.hasrequirements && !StoryManager.Instance.HasMarker(currentLine.StoryMarkerRequirement))
            {
                OnDialogueFinished(false); 
                return;
            }

            // Normal progression: increment FIRST, then save the progress, then check and display
            if (_dialogueIndex < _currentDialogueSo.DialogueArray.Length - 1)
            {
                _dialogueIndex++;
                _dialogueProgress[_currentDialogueSo] = _dialogueIndex; // <--- SAVE PROGRESS
                CheckDialogue();
            }
            else
            {
                OnDialogueFinished(true);
            }
        }

        public void NextDialogue()
        {
            if (!_hasActiveDialogue || _isTransitioning)
                return;

            if (_currentDialogueSo == null)
            {
                _hasActiveDialogue = false;
                return;
            }

            if (_currentDialogueSo.assignedType == DialogueType.SequentialDialogue)
            {
                IncrementDialogue();
            }
            else if (_currentDialogueSo.assignedType == DialogueType.ItemDialogue)
            {
                OnDialogueFinished(true);
            }
        }

        private void CheckDialogue()
        {
            if (_currentDialogueSo.DialogueArray[_dialogueIndex].hasrequirements)
            {
                if (StoryManager.Instance.HasMarker(_currentDialogueSo.DialogueArray[_dialogueIndex].StoryMarkerRequirement))
                {
                    dialogueStruct = _currentDialogueSo.DialogueArray[_dialogueIndex];
                }
                else
                {
                    dialogueStruct =  _currentDialogueSo.LockedDialogue;
                }
            }
            else
            {
                dialogueStruct = _currentDialogueSo.DialogueArray[_dialogueIndex];
            }
            DisplayDialogue();
        }

        public void SetSequentialDialogue(DialogueSO dialogueSo)
        {
            if (dialogueSo == null)
            {
                _hasActiveDialogue = false;
                return;
            }
            
            _currentDialogueSo = dialogueSo;
            _hasActiveDialogue = true;
            
            // LOAD PROGRESS: Check if we've talked to this NPC before
            if (_dialogueProgress.TryGetValue(dialogueSo, out int savedIndex))
            {
                _dialogueIndex = savedIndex;
            }
            else
            {
                // First time talking to this NPC
                _dialogueIndex = 0;
                _dialogueProgress[dialogueSo] = 0;
            }

            CheckDialogue();
        }
        
        public void SetRandomDialogue(DialogueSO dialogueSo)
        {
            if (dialogueSo == null)
            {
                _hasActiveDialogue = false;
                return;
            }
            _currentDialogueSo = dialogueSo;
            _hasActiveDialogue = true;
            _dialogueIndex = Random.Range(0, _currentDialogueSo.DialogueArray.Length);
            CheckDialogue();
        }

        private void DisplayDialogue()
        {
            if (_currentDialogueSo == null ||
                _currentDialogueSo.DialogueArray == null ||
                _currentDialogueSo.DialogueArray.Length == 0)
            {
                _hasActiveDialogue = false;
                return;
            }
            if (_currentDialogueSo.assignedType == DialogueType.ItemDialogue)
            {
                DisplayClueSequential();
                return;
            }
            _dialogueIndex = Mathf.Clamp(_dialogueIndex, 0, _currentDialogueSo.DialogueArray.Length - 1);
            var d = dialogueStruct;
            UIManager.Instance.DisplayToast(d);
        }

        private void DisplayClueSequential()
        {
            if (_dialogueIndex < _currentDialogueSo.DialogueArray.Length - 1)
                _dialogueIndex++;
            else
                _dialogueIndex = 0;
            
            _dialogueIndex = Mathf.Clamp(_dialogueIndex, 0, _currentDialogueSo.DialogueArray.Length - 1);
            var d = dialogueStruct;
            UIManager.Instance.DisplayClueHUD(d);
        }
        
        private void OnDialogueFinished(bool success = true)
        {
            var currentLine = _currentDialogueSo.DialogueArray[_dialogueIndex];

            // Only produce a marker if the player successfully completed the dialogue requirements
            // AND we ensure they don't already have the produced marker
            if (success && currentLine.hasrequirements && currentLine.StoryMarkerProduced != null)
            {
                if (!StoryManager.Instance.HasMarker(currentLine.StoryMarkerProduced))
                {
                    EventManager.instance.Publish(new StoryMarkerUnlockedEvent(currentLine.StoryMarkerProduced));
                }
            }
            
            _isTransitioning = true;
            _hasActiveDialogue = false;
            GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
            _isTransitioning = false;
            EventManager.instance.Publish(new DialogueFinishedEvent());
        }
    }
}