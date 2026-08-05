using System;
using System.Collections.Generic;
using EventSystem;
using Story;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField] private Button nextButton;
        private int _dialogueIndex;
        private DialogueSO _currentDialogueSo;
        public int DialogueIndex => _dialogueIndex;
        public DialogueSO CurrentDialogueSo => _currentDialogueSo;
        
        private bool _isTransitioning;
        private bool _hasActiveDialogue;
        public List<StoryMarker> MarkersUnlocked = new List<StoryMarker>();

        private void IncrementDialogue()
        {
            if (!_hasActiveDialogue || _currentDialogueSo == null)
                return;

            if (_currentDialogueSo.DialogueArray == null || _currentDialogueSo.DialogueArray.Length == 0)
            {
                OnDialogueFinished();
                return;
            }

            if (_dialogueIndex < _currentDialogueSo.DialogueArray.Length - 1)
            {
                _dialogueIndex++;
                DisplayDialogue();
            }
            else
            {
                OnDialogueFinished();
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
                OnDialogueFinished();
            }
            else
            {
                GameManager.Instance.playerStateMachine.changeState(
                    GameManager.Instance.playerStateMachine.idlestate);
                _hasActiveDialogue = false;
            }
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
            _dialogueIndex = 0;
            DisplayDialogue();
        }

        public void ContinueDialogue()
        {
            _hasActiveDialogue = true;
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
            DisplayDialogue();
        }

        private void DisplayLockedDialogue()
        {
            if (_currentDialogueSo == null)
            {
                return;
            }
            _hasActiveDialogue = true;
            var d = _currentDialogueSo.LockedDialogue;
            UIManager.Instance.DisplayToast(d);
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
            if (!_currentDialogueSo.DialogueArray[_dialogueIndex].requirementFufilled)
            {
                DisplayLockedDialogue();
                return;
            }
            if (_currentDialogueSo.assignedType == DialogueType.ItemDialogue)
            {
                DisplayClueSequential();
                return;
            }
            _dialogueIndex = Mathf.Clamp(_dialogueIndex, 0, _currentDialogueSo.DialogueArray.Length - 1);
            var d = _currentDialogueSo.DialogueArray[_dialogueIndex];
            UIManager.Instance.DisplayToast(d);
        }

        private void DisplayClueSequential()
        {
            if (_dialogueIndex < _currentDialogueSo.DialogueArray.Length - 1)
                _dialogueIndex++;
            else
                _dialogueIndex = 0;
            _dialogueIndex = Mathf.Clamp(_dialogueIndex, 0, _currentDialogueSo.DialogueArray.Length - 1);
            var d = _currentDialogueSo.DialogueArray[_dialogueIndex];
            UIManager.Instance.DisplayClueHUD(d);
        }
        
        public void OnDialogueFinished()
        {
            if ( !StoryManager.Instance.HasMarker(_currentDialogueSo.DialogueArray[_dialogueIndex].StoryMarkerRequirement))
            {
                EventManager.instance.Publish(new StoryMarkerUnlockedEvent(_currentDialogueSo.DialogueArray[_dialogueIndex].StoryMarkerRequirement));
            }
            
            _isTransitioning = true;
            _hasActiveDialogue = false;
            GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
            _isTransitioning = false;
            EventManager.instance.Publish(new DialogueFinishedEvent());
        }
    }
}