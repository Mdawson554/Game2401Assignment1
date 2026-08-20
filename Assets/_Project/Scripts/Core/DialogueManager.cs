using System.Collections.Generic;
using _Project.ScriptableObjects;
using _Project.Scripts.EventSystem;
using _Project.Scripts.Gameplay;
using _Project.Scripts.Interactions;
using _Project.Scripts.Story;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core
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

            if (line.RequiredMarkers != null &&
                !StoryManager.Instance.HasMarker(line.RequiredMarkers))
                return false;

            return true;
        }

        public void ProduceMarkers(DialogueStruct line)
        {
            if (line.ProducedMarkers == null)
            {
                return;
            }
            StoryManager storyManager = StoryManager.Instance;

            if (line.HasRequirements &&
                !storyManager.HasMarker(line.RequiredMarkers))
            {
                return;
            }

            EventManager.instance.Publish(
                new StoryMarkerUnlockedEvent(line.ProducedMarkers)
            );
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
            bool isItemDialogue = _currentDialogueSo != null && _currentDialogueSo.assignedType == DialogueType.ItemDialogue;
            bool isSequentialDialogue = _currentDialogueSo != null && _currentDialogueSo.assignedType == DialogueType.SequentialDialogue;

            if (dialogueStruct.HasRequirements)
            {
                if (StoryManager.Instance.HasMarker(dialogueStruct.RequiredMarkers))
                {
                    if (isItemDialogue)
                    {
                        UIManager.Instance.DisplayClueHUD(dialogueStruct);
                    }
                    else
                    {
                        UIManager.Instance.DisplayToast(dialogueStruct, isSequentialDialogue);
                    }
                }
                else
                {
                    if (isItemDialogue)
                    {
                        UIManager.Instance.DisplayLockedClueHUD(dialogueStruct);
                    }
                    else
                    {
                        UIManager.Instance.DisplayLockedToast(dialogueStruct);
                    }
                }
            }
            else if (!dialogueStruct.HasRequirements)
            {
                if (isItemDialogue)
                {
                    UIManager.Instance.DisplayClueHUD(dialogueStruct);
                }
                else
                {
                    UIManager.Instance.DisplayToast(dialogueStruct, isSequentialDialogue);
                }
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
            if (_dialogueSoSavedIndexs.TryGetValue(dialogueSo, out int savedIndexValue))
            {
                _dialogueIndex = savedIndexValue;
            }
            else
            {
                _dialogueIndex = 0;
            }
            SetDialogueStruct();
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
            SetDialogueStruct();
            CheckDialogue();
        }

        private void TryProduceMarker()
        {
            ProduceMarkers(dialogueStruct);
        }

        private void OnDialogueFinished()
        {
            _dialogueSoSavedIndexs[_currentDialogueSo] = _dialogueIndex;
            _isTransitioning = true;
            _hasActiveDialogue = false;
            GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
            _isTransitioning = false;
            EventManager.instance.Publish(new DialogueFinishedEvent());
            TryProduceMarker();
        }
        
        [ContextMenu("Print Dialogue State")]
        private void PrintDialogueState()
        {
            Debug.Log(
                $"[Dialogue State]\n" +
                $"Active Dialogue SO: {_currentDialogueSo}\n" +
                $"Dialogue Index: {_dialogueIndex}\n" +
                $"Has Active Dialogue: {_hasActiveDialogue}\n" +
                $"Is Transitioning: {_isTransitioning}\n" +
                $"Current Line: {(dialogueStruct != null ? dialogueStruct.Dialogue : "null")}\n" +
                $"Requirements Met: {RequirementsMet(dialogueStruct)}\n" +
                $"Produced Marker: {(dialogueStruct?.ProducedMarkers ? dialogueStruct.ProducedMarkers.name : "none")}"
            );
        }

    }
}