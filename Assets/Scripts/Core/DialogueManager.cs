using EventSystem;
using UnityEngine;
using UnityEngine.UI;

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
            if (dialogueSo.DialogueArray == null || dialogueSo.DialogueArray.Length == 0)
            {
                _hasActiveDialogue = false;
                return;
            }
            if (dialogueSo.assignedType == DialogueType.ItemDialogue)
            {
                DisplayClueSequential();
                return;
            }
            _dialogueIndex = 0;
            DisplayDialogue();
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

            if (dialogueSo.DialogueArray == null || dialogueSo.DialogueArray.Length == 0)
            {
                _hasActiveDialogue = false;
                return;
            }

            _dialogueIndex = Random.Range(0, _currentDialogueSo.DialogueArray.Length);
            DisplayDialogue();
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
            _dialogueIndex = Mathf.Clamp(_dialogueIndex, 0, _currentDialogueSo.DialogueArray.Length - 1);
            var d = _currentDialogueSo.DialogueArray[_dialogueIndex];
            UIManager.Instance.DisplayToast(d);
        }

        private void DisplayClueSequential()
        {
            if (_currentDialogueSo == null ||
                _currentDialogueSo.DialogueArray == null ||
                _currentDialogueSo.DialogueArray.Length == 0)
            {
                _hasActiveDialogue = false;
                return;
            }
            if (_dialogueIndex < _currentDialogueSo.DialogueArray.Length - 1)
                _dialogueIndex++;
            else
                _dialogueIndex = 0;
            _dialogueIndex = Mathf.Clamp(_dialogueIndex, 0, _currentDialogueSo.DialogueArray.Length - 1);
            var d = _currentDialogueSo.DialogueArray[_dialogueIndex];
            UIManager.Instance.DisplayClueHUD(d);
        }

        private void OnDialogueFinished()
        {
            _isTransitioning = true;
            _hasActiveDialogue = false;

            GameManager.Instance.playerStateMachine.changeState(
                GameManager.Instance.playerStateMachine.idlestate);

            EventManager.instance.Publish(new DialogueFinishedEvent());

            _isTransitioning = false;
        }
    }
}