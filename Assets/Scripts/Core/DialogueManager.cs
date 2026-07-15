using System;
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
        public static DialogueManager Instance;
        
        [SerializeField] private Button _nextButton;
        public int dialogueindex = 0;
        
        public DialogueSO currentdialogueSO;
    
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }
        
        public void incrementDialogue()
        {
            if (dialogueindex < currentdialogueSO.DialogueArray.Length - 1)
            {
                dialogueindex++;
            }
            else if (dialogueindex >= currentdialogueSO.DialogueArray.Length)
            {
                OnDialogueFinished();
            }
        }

        public void SetSequentialDialogue(DialogueSO dialogueSO, InteractableObjectTypes interactableObjectType)
        {
            //set the current dialogue in sequential order
            currentdialogueSO = dialogueSO;

            switch (interactableObjectType)
            {
                case InteractableObjectTypes.NPC:
                    DisplayDialogue();
                    break;
                case InteractableObjectTypes.Clues:
                    DisplayItemDialogue();
                    break;
                case InteractableObjectTypes.Keys:
                    DisplayItemDialogue();
                    break;
            }
            
        }

        public void SetRandomDialogue(DialogueSO dialogueSO)
        {
            //set the current dialogue in random order
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
        }
    }
}
