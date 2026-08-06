using System;
using System.Collections.Generic;
using EventSystem;
using Gameplay;
using Story;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        private int _dialogueIndex;
        private bool _isTransitioning;
        private bool _hasActiveDialogue;
        private DialogueSO _currentDialogueSo;
        
        public DialogueStruct dialogueStruct;

        private void IncrementDialogue()
        {
            CheckDialogue();
            if(!StoryManager.Instance.HasMarker(_currentDialogueSo.DialogueArray[_dialogueIndex].StoryMarkerRequirement) && _currentDialogueSo.DialogueArray[_dialogueIndex].hasrequirements)
            {
                _isTransitioning = true;
                _hasActiveDialogue = false;
                GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
                _isTransitioning = false;
            }
            if (_dialogueIndex < _currentDialogueSo.DialogueArray.Length - 1 || !_currentDialogueSo.DialogueArray[_dialogueIndex].hasrequirements )
            {
                _dialogueIndex++;
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

            Debug.Log("incr");

            if (_currentDialogueSo.assignedType == DialogueType.SequentialDialogue)
            {
                IncrementDialogue();
            }
            else if (_currentDialogueSo.assignedType == DialogueType.ItemDialogue)
            {
                OnDialogueFinished();
            }
        }


        private void CheckDialogue()
        {
            if (_currentDialogueSo.DialogueArray[_dialogueIndex].hasrequirements)
            {
                if (StoryManager.Instance.HasMarker(_currentDialogueSo.DialogueArray[_dialogueIndex].StoryMarkerRequirement))
                {
                    Debug.Log("Unlocked");
                    dialogueStruct = _currentDialogueSo.DialogueArray[_dialogueIndex ];
                }
                else
                {
                    Debug.Log("locked");
                    dialogueStruct =  _currentDialogueSo.LockedDialogue;
                }
            }
            else
            {
                dialogueStruct = _currentDialogueSo.DialogueArray[_dialogueIndex ];
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
            _dialogueIndex = 0;
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
        
        private void OnDialogueFinished()
        {
            if (_currentDialogueSo.DialogueArray[_dialogueIndex].hasrequirements)
            {
                if ( !StoryManager.Instance.HasMarker(_currentDialogueSo.DialogueArray[_dialogueIndex].StoryMarkerRequirement ))
                {
                    EventManager.instance.Publish(new StoryMarkerUnlockedEvent(_currentDialogueSo.DialogueArray[_dialogueIndex].StoryMarkerProduced));
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