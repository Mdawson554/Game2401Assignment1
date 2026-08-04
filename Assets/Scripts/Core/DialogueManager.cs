using EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField] private Button nextButton;
        public int dialogueIndex;
        public DialogueSO currentDialogueSo;
        private bool _isTransitioning;
        private bool _hasActiveDialogue;

        private void IncrementDialogue()
        {
            if (!_hasActiveDialogue || currentDialogueSo == null)
                return;

            if (currentDialogueSo.DialogueArray == null || currentDialogueSo.DialogueArray.Length == 0)
            {
                OnDialogueFinished();
                return;
            }

            if (dialogueIndex < currentDialogueSo.DialogueArray.Length - 1)
            {
                dialogueIndex++;
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

            if (currentDialogueSo == null)
            {
                _hasActiveDialogue = false;
                return;
            }

            if (currentDialogueSo.assignedType == DialogueType.SequentialDialogue)
            {
                IncrementDialogue();
            }
            else if (currentDialogueSo.assignedType == DialogueType.ItemDialogue)
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

            currentDialogueSo = dialogueSo;
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

            dialogueIndex = 0;
            DisplayDialogue();
        }

        public void SetRandomDialogue(DialogueSO dialogueSo)
        {
            if (dialogueSo == null)
            {
                _hasActiveDialogue = false;
                return;
            }

            currentDialogueSo = dialogueSo;
            _hasActiveDialogue = true;

            if (dialogueSo.DialogueArray == null || dialogueSo.DialogueArray.Length == 0)
            {
                _hasActiveDialogue = false;
                return;
            }

            dialogueIndex = Random.Range(0, currentDialogueSo.DialogueArray.Length);
            DisplayDialogue();
        }

        private void DisplayDialogue()
        {
            if (currentDialogueSo == null ||
                currentDialogueSo.DialogueArray == null ||
                currentDialogueSo.DialogueArray.Length == 0)
            {
                _hasActiveDialogue = false;
                return;
            }
            dialogueIndex = Mathf.Clamp(dialogueIndex, 0, currentDialogueSo.DialogueArray.Length - 1);
            var d = currentDialogueSo.DialogueArray[dialogueIndex];
            UIManager.Instance.DisplayToast(d);
        }

        private void DisplayClueSequential()
        {
            if (currentDialogueSo == null ||
                currentDialogueSo.DialogueArray == null ||
                currentDialogueSo.DialogueArray.Length == 0)
            {
                _hasActiveDialogue = false;
                return;
            }
            if (dialogueIndex < currentDialogueSo.DialogueArray.Length - 1)
                dialogueIndex++;
            else
                dialogueIndex = 0;
            dialogueIndex = Mathf.Clamp(dialogueIndex, 0, currentDialogueSo.DialogueArray.Length - 1);
            var d = currentDialogueSo.DialogueArray[dialogueIndex];
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
