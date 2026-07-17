using TMPro;
using Gameplay;
using Interactions;
using States.StateTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace Core
{
    public class DialogueManager : MonoBehaviour
    { 
        [SerializeField] private Button _nextButton;
        
        public static DialogueManager Instance;
        public int dialogueindex = 0;
        public DialogueSO currentdialogueSO; 
    
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }
        
        public void incrementDialogue()
        {
            // Check if there's a NEXT dialogue before incrementing
            if (dialogueindex < currentdialogueSO.DialogueArray.Length - 1)
            {
                dialogueindex++;
                DisplayDialogue();
            }
            else
            {
                // We're at the last dialogue
                OnDialogueFinished();
            }
        }
        
        public void NextDialogue()
        {
            if (currentdialogueSO.assignedType == DialogueType.SequentialDialogue)
            {
                incrementDialogue();
            }
            else
            {
                GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
            }
        }
        
        public void SetSequentialDialogue(DialogueSO dialogueSO)
        {
            currentdialogueSO = dialogueSO;
            dialogueindex = 0;
            var tempDialogue = currentdialogueSO.assignedType;
            switch (tempDialogue)
            {
                case DialogueType.ItemDialogue :
                    DisplayItemDialogue();
                    break;
                case DialogueType.SequentialDialogue :
                    DisplayDialogue();
                    break;
                default:
                    DisplayDialogue();
                    break;
            }
        }
        
        public void SetRandomDialogue(DialogueSO dialogueSO)
        {
            currentdialogueSO = dialogueSO;
            int random = UnityEngine.Random.Range (0, currentdialogueSO.DialogueArray.Length);
            dialogueindex = random;
            DisplayDialogue();
        }
        
        public void DisplayDialogue()
        {
            UIManager.Instance.DisplayToast(currentdialogueSO.DialogueArray[dialogueindex].Dialogue);
        }
        
        public void DisplayItemDialogue()
        {
            dialogueindex = 0;
            UIManager.Instance.DisplayClueHUD(currentdialogueSO.DialogueArray[dialogueindex].Dialogue);
        }
        
        private void OnDialogueFinished()
        {
            GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
            Debug.Log("this NPC Dialogue is done");
        }
    }
}