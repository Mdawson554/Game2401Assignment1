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

        private Dictionary<DialogueStruct, int> _dialogueProgress = new Dictionary<DialogueStruct, int>();
        private Dictionary<DialogueSO, int> _dialogueSoSavedIndexs = new Dictionary<DialogueSO, int>();

        public DialogueStruct dialogueStruct;
        
        public bool RequirementsMet(DialogueStruct line)
        {
            if (!line.HasRequirements)
                return true;
            
            if (dialogueStruct.RequiredMarkers != null && !StoryManager.Instance.HasMarker(dialogueStruct.RequiredMarkers))
                return false;
            
            return true;
        }

        public void ProduceMarkers(DialogueStruct line)
        {
            if (line.ProducedMarkers == null)
                return;
            if (!StoryManager.Instance.HasMarker(dialogueStruct.RequiredMarkers))
                EventManager.instance.Publish(new StoryMarkerUnlockedEvent(dialogueStruct.ProducedMarkers));
        }

        private void IncrementDialogue()
        {
            if (_dialogueIndex < _currentDialogueSo.DialogueArray.Length - 1)
            {
                if (!dialogueStruct.HasRequirements || (StoryManager.Instance.HasMarker(dialogueStruct.RequiredMarkers)))
                {
                    _dialogueIndex++;
                    TryProduceMarker();
                    SetDialogueStruct();
                    CheckDialogue();
                }
                else
                {
                   CheckDialogue();
                   SetDialogueStruct();
                    OnDialogueFinished();
                    return;
                }
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
                IncrementDialogue();
            else if (_currentDialogueSo.assignedType == DialogueType.ItemDialogue)
                OnDialogueFinished();
        }

        private void CheckDialogue()
        {
            if (dialogueStruct.HasRequirements)
            {
                if (StoryManager.Instance.HasMarker(dialogueStruct.RequiredMarkers))
                {
                    Debug.Log("requirementMet");
                    UIManager.Instance.DisplayToast(dialogueStruct);
                }
                else
                {
                    Debug.Log("requirements not met");
                    UIManager.Instance.DisplayLockedToast(dialogueStruct);
                }
            }
            else if (!dialogueStruct.HasRequirements)
            {
                Debug.Log("no requirment");
                UIManager.Instance.DisplayToast(dialogueStruct);
            }
            
        }

        public void SetSequentialDialogue(DialogueSO dialogueSo)
        {
            if (dialogueSo == null)
            {
                _hasActiveDialogue = false;
                return;
            }
            _dialogueIndex = 0;
            _currentDialogueSo = dialogueSo;
            _hasActiveDialogue = true;


            if (_dialogueSoSavedIndexs.TryGetValue(dialogueSo, out int savedIndexValue))
            {
               
                _dialogueIndex = savedIndexValue;
                Debug.Log(savedIndexValue);
                SetDialogueStruct();
            }
            else
            {
                Debug.Log(_dialogueSoSavedIndexs);
                SetDialogueStruct();
            }
            CheckDialogue();
            
        }

        private void SetDialogueStruct()
        {
            dialogueStruct = _currentDialogueSo.DialogueArray[_dialogueIndex];
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

        // private void DisplayDialogue()
        // {
        //     if (_currentDialogueSo == null ||
        //         _currentDialogueSo.DialogueArray == null ||
        //         _currentDialogueSo.DialogueArray.Length == 0)
        //     {
        //         _hasActiveDialogue = false;
        //         return;
        //     }
        //     if (_currentDialogueSo.assignedType == DialogueType.ItemDialogue)
        //     {
        //         DisplayClueSequential();
        //         return;
        //     }
        //     // _dialogueIndex = Mathf.Clamp(_dialogueIndex, 0, _currentDialogueSo.DialogueArray.Length - 1);
        //     //UIManager.Instance.DisplayToast(dialogueStruct);
        // }

        private void DisplayClueSequential()
        {
            if (_dialogueIndex < _currentDialogueSo.DialogueArray.Length - 1)
                _dialogueIndex++;
            else
                _dialogueIndex = 0;

            _dialogueIndex = Mathf.Clamp(_dialogueIndex, 0, _currentDialogueSo.DialogueArray.Length - 1);
            UIManager.Instance.DisplayClueHUD(dialogueStruct);
        }

        private void TryProduceMarker()
        {
            if (StoryManager.Instance.HasMarker(dialogueStruct.RequiredMarkers))
            {
                EventManager.instance.Publish(new StoryMarkerUnlockedEvent(dialogueStruct.ProducedMarkers));
            }
        }

        private void OnDialogueFinished()
        {
            _dialogueSoSavedIndexs.TryAdd(_currentDialogueSo, _dialogueIndex);
            _isTransitioning = true;
            _hasActiveDialogue = false;
            GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
            _isTransitioning = false;
            EventManager.instance.Publish(new DialogueFinishedEvent());
            TryProduceMarker();
        }
    }
}
