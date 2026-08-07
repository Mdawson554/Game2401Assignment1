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

        private Dictionary<DialogueSO, int> _dialogueProgress = new Dictionary<DialogueSO, int>();

        public DialogueStruct dialogueStruct;
        
        public bool RequirementsMet(DialogueStruct line)
        {
            if (!line.HasRequirements)
                return true;

            foreach (var marker in line.RequiredMarkers)
            {
                if (marker != null && !StoryManager.Instance.HasMarker(marker))
                    return false;
            }

            return true;
        }

        public void ProduceMarkers(DialogueStruct line, bool success)
        {
            if (!success || line.ProducedMarkers == null)
                return;

            foreach (var marker in line.ProducedMarkers)
            {
                if (marker != null && !StoryManager.Instance.HasMarker(marker))
                    EventManager.instance.Publish(new StoryMarkerUnlockedEvent(marker));
            }
        }

        private void IncrementDialogue()
        {
            var currentLine = _currentDialogueSo.DialogueArray[_dialogueIndex];

            if (currentLine.HasRequirements && !RequirementsMet(currentLine))
            {
                OnDialogueFinished(false);
                return;
            }

            if (_dialogueIndex < _currentDialogueSo.DialogueArray.Length - 1)
            {
                _dialogueIndex++;
                _dialogueProgress[_currentDialogueSo] = _dialogueIndex;
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
                IncrementDialogue();
            else if (_currentDialogueSo.assignedType == DialogueType.ItemDialogue)
                OnDialogueFinished(true);
        }

        private void CheckDialogue()
        {
            var line = _currentDialogueSo.DialogueArray[_dialogueIndex];

            if (line.HasRequirements)
            {
                if (RequirementsMet(line))
                {
                    dialogueStruct = line;
                }
                else
                {
                    dialogueStruct = new DialogueStruct
                    {
                        Dialogue = line.LockedDialogue,
                        DialogueColor = line.LockedDialogueColor,
                        DialogueSprite = line.DialogueSprite,
                        DialogueFont = line.DialogueFont,
                        DialogueText = line.DialogueText,
                        Type = line.Type
                    };
                }
            }
            else
            {
                dialogueStruct = line;
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

            if (_dialogueProgress.TryGetValue(dialogueSo, out int savedIndex))
                _dialogueIndex = savedIndex;
            else
            {
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
            UIManager.Instance.DisplayToast(dialogueStruct);
        }

        private void DisplayClueSequential()
        {
            if (_dialogueIndex < _currentDialogueSo.DialogueArray.Length - 1)
                _dialogueIndex++;
            else
                _dialogueIndex = 0;

            _dialogueIndex = Mathf.Clamp(_dialogueIndex, 0, _currentDialogueSo.DialogueArray.Length - 1);
            UIManager.Instance.DisplayClueHUD(dialogueStruct);
        }

        private void OnDialogueFinished(bool success = true)
        {
            var currentLine = _currentDialogueSo.DialogueArray[_dialogueIndex];
            ProduceMarkers(currentLine, success);
            _isTransitioning = true;
            _hasActiveDialogue = false;
            GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
            _isTransitioning = false;
            EventManager.instance.Publish(new DialogueFinishedEvent());
        }
    }
}
