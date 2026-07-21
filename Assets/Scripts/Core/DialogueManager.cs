using EventSystem;
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
            if (dialogueindex < currentdialogueSO.DialogueArray.Length - 1)
            {
                dialogueindex++;
                DisplayDialogue();
            }
            else
            {
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
            if (dialogueSO.assignedType == DialogueType.ItemDialogue)
            {
                DisplayClueSequential();
                return;
            }

            dialogueindex = 0;
            DisplayDialogue();
        }

        public void SetRandomDialogue(DialogueSO dialogueSO)
        {
            currentdialogueSO = dialogueSO;
            int random = UnityEngine.Random.Range(0, currentdialogueSO.DialogueArray.Length);
            dialogueindex = random;
            DisplayDialogue();
        }

        public void DisplayDialogue()
        {
            UIManager.Instance.DisplayToast(currentdialogueSO.DialogueArray[dialogueindex].Dialogue);
        }
        
        private void DisplayClueSequential()
        {
            if (dialogueindex < currentdialogueSO.DialogueArray.Length - 1)
            {
                dialogueindex++;
            }
            else
            {
                dialogueindex = 0;
            }

            UIManager.Instance.DisplayClueHUD(currentdialogueSO.DialogueArray[dialogueindex].Dialogue);
        }

        private void OnDialogueFinished()
        {
            GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
            Debug.Log("this NPC Dialogue is done");
            EventManager.instance.Publish(new DialogueFinishedEvent());
        }
    }
}
