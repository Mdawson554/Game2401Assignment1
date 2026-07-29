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
            {
                return;
            }
            if (currentDialogueSo == null)
            {
                return;
            }
            if (currentDialogueSo.DialogueArray == null || currentDialogueSo.DialogueArray.Length == 0)
            {
                OnDialogueFinished();
                return;
            }
            if (dialogueIndex < 0 || dialogueIndex >= currentDialogueSo.DialogueArray.Length)
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
            if (!_hasActiveDialogue)
            {
                return;
            }
            if (_isTransitioning)
            {
                return;
            }
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
                if (GameManager.Instance != null && GameManager.Instance.playerStateMachine != null)
                {
                    GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
                }
                else
                {
                    
                }
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
            
            int random = Random.Range(0, currentDialogueSo.DialogueArray.Length);
            dialogueIndex = random;
            DisplayDialogue();
        }
        
        private void DisplayDialogue()
        {
            if (currentDialogueSo == null)
            {
                _hasActiveDialogue = false;
                return;
            }
            if (currentDialogueSo.DialogueArray == null || currentDialogueSo.DialogueArray.Length == 0)
            {
                _hasActiveDialogue = false;
                return;
            }
            if (dialogueIndex < 0 || dialogueIndex >= currentDialogueSo.DialogueArray.Length)
            {
                dialogueIndex = Mathf.Clamp(dialogueIndex, 0, currentDialogueSo.DialogueArray.Length - 1);
            }
            if (UIManager.Instance == null)
            {
                _hasActiveDialogue = false;
                return;
            }
            
            UIManager.Instance.DisplayToast(currentDialogueSo.DialogueArray[dialogueIndex].Dialogue);
        }
        
        private void DisplayClueSequential()
        {
            if (currentDialogueSo == null)
            {
                _hasActiveDialogue = false;
                return;
            }
            if (currentDialogueSo.DialogueArray == null || currentDialogueSo.DialogueArray.Length == 0)
            {
                _hasActiveDialogue = false;
                return;
            }
            
            if (dialogueIndex < currentDialogueSo.DialogueArray.Length - 1)
            {
                dialogueIndex++;
            }
            else
            {
                dialogueIndex = 0;
            }
            dialogueIndex = Mathf.Clamp(dialogueIndex, 0, currentDialogueSo.DialogueArray.Length - 1);
            if (UIManager.Instance == null)
            {
                _hasActiveDialogue = false;
                return;
            }
            
            UIManager.Instance.DisplayClueHUD(currentDialogueSo.DialogueArray[dialogueIndex].Dialogue);
        }
        
        private void OnDialogueFinished()
        {
            _isTransitioning = true;
            _hasActiveDialogue = false;  
            if (GameManager.Instance == null)
            {
                _isTransitioning = false;
                return;
            }
            if (GameManager.Instance.playerStateMachine == null)
            {
                _isTransitioning = false;
                return;
            }
            
            GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
            if (EventManager.instance != null)
            {
                EventManager.instance.Publish(new DialogueFinishedEvent());
            }
            else
            {
            }
            
            _isTransitioning = false;
        }
    }
}