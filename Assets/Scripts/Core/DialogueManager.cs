using System;
using Gameplay;
using UnityEngine;

namespace Core
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        public int dialogueindex = 0;
        
        public DialogueSO currentdialogueSO;
        private DialogueStruct _currentDialogueStruct ;
    
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
            else
            {
                return;
            }
        }

        public void setessentialNPCDialogue()
        {
            //set the current dialogue in sequential order
            _currentDialogueStruct = currentdialogueSO.DialogueArray [dialogueindex%currentdialogueSO.DialogueArray.Length];
            DisplayDialogue();
        }

        public void setstandardNPCDialogue()
        {
            //set the current dialogue in random order
            int random = UnityEngine.Random.Range (0, currentdialogueSO.DialogueArray.Length);
            dialogueindex = random;
            _currentDialogueStruct = currentdialogueSO.DialogueArray [dialogueindex%currentdialogueSO.DialogueArray.Length];
            DisplayDialogue();
        }
        
        public void DisplayDialogue()
        {
            UIManager.Instance.DisplayToast(currentdialogueSO.DialogueArray[dialogueindex].Dialogue);
        }
    }
}
