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
        private bool _isTransitioning = false;
        private bool _hasActiveDialogue = false;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }
        
        public void incrementDialogue()
        {
            if (!_hasActiveDialogue || currentdialogueSO == null)
            {
                Debug.LogWarning("[DialogueManager] incrementDialogue called but no active dialogue. Ignoring.");
                return;
            }
            if (currentdialogueSO == null)
            {
                Debug.LogError("[DialogueManager] currentdialogueSO is null during incrementDialogue");
                return;
            }
            if (currentdialogueSO.DialogueArray == null || currentdialogueSO.DialogueArray.Length == 0)
            {
                Debug.LogError("[DialogueManager] DialogueArray is null or empty");
                OnDialogueFinished();
                return;
            }
            if (dialogueindex < 0 || dialogueindex >= currentdialogueSO.DialogueArray.Length)
            {
                Debug.LogError($"[DialogueManager] dialogueindex {dialogueindex} is out of bounds for DialogueArray (length: {currentdialogueSO.DialogueArray.Length})");
                OnDialogueFinished();
                return;
            }
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
            if (!_hasActiveDialogue)
            {
                Debug.LogWarning("[DialogueManager] NextDialogue pressed but no active dialogue. Ignoring.");
                return;
            }
            if (_isTransitioning)
            {
                Debug.LogWarning("[DialogueManager] Ignoring NextDialogue - already transitioning");
                return;
            }
            if (currentdialogueSO == null)
            {
                Debug.LogError("[DialogueManager] currentdialogueSO is null during NextDialogue");
                _hasActiveDialogue = false;
                return;
            }
            
            if (currentdialogueSO.assignedType == DialogueType.SequentialDialogue)
            {
                incrementDialogue();
            }
            else if (currentdialogueSO.assignedType == DialogueType.ItemDialogue)
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
                    Debug.LogError("[DialogueManager] GameManager or playerStateMachine is null");
                }
                _hasActiveDialogue = false;
            }
        }
        
        public void SetSequentialDialogue(DialogueSO dialogueSO)
        {
            if (dialogueSO == null)
            {
                Debug.LogError("[DialogueManager] Attempted to set null DialogueSO as sequential");
                _hasActiveDialogue = false;
                return;
            }
            
            currentdialogueSO = dialogueSO;
            _hasActiveDialogue = true;  
            if (dialogueSO.DialogueArray == null || dialogueSO.DialogueArray.Length == 0)
            {
                Debug.LogError($"[DialogueManager] DialogueSO '{dialogueSO.name}' has null or empty DialogueArray");
                _hasActiveDialogue = false;
                return;
            }
            
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
            if (dialogueSO == null)
            {
                Debug.LogError("[DialogueManager] Attempted to set null DialogueSO as random");
                _hasActiveDialogue = false;
                return;
            }
            
            currentdialogueSO = dialogueSO;
            _hasActiveDialogue = true;  
            if (dialogueSO.DialogueArray == null || dialogueSO.DialogueArray.Length == 0)
            {
                Debug.LogError($"[DialogueManager] DialogueSO '{dialogueSO.name}' has null or empty DialogueArray");
                _hasActiveDialogue = false;
                return;
            }
            
            int random = Random.Range(0, currentdialogueSO.DialogueArray.Length);
            dialogueindex = random;
            DisplayDialogue();
        }
        
        public void DisplayDialogue()
        {
            if (currentdialogueSO == null)
            {
                Debug.LogError("[DialogueManager] currentdialogueSO is null during DisplayDialogue");
                _hasActiveDialogue = false;
                return;
            }
            if (currentdialogueSO.DialogueArray == null || currentdialogueSO.DialogueArray.Length == 0)
            {
                Debug.LogError("[DialogueManager] DialogueArray is null or empty");
                _hasActiveDialogue = false;
                return;
            }
            if (dialogueindex < 0 || dialogueindex >= currentdialogueSO.DialogueArray.Length)
            {
                Debug.LogError($"[DialogueManager] dialogueindex {dialogueindex} is out of bounds (array length: {currentdialogueSO.DialogueArray.Length})");
                dialogueindex = Mathf.Clamp(dialogueindex, 0, currentdialogueSO.DialogueArray.Length - 1);
            }
            if (UIManager.Instance == null)
            {
                Debug.LogError("[DialogueManager] UIManager.Instance is null");
                _hasActiveDialogue = false;
                return;
            }
            
            UIManager.Instance.DisplayToast(currentdialogueSO.DialogueArray[dialogueindex].Dialogue);
        }
        
        private void DisplayClueSequential()
        {
            if (currentdialogueSO == null)
            {
                Debug.LogError("[DialogueManager] currentdialogueSO is null during DisplayClueSequential");
                _hasActiveDialogue = false;
                return;
            }
            if (currentdialogueSO.DialogueArray == null || currentdialogueSO.DialogueArray.Length == 0)
            {
                Debug.LogError("[DialogueManager] DialogueArray is null or empty in DisplayClueSequential");
                _hasActiveDialogue = false;
                return;
            }
            
            if (dialogueindex < currentdialogueSO.DialogueArray.Length - 1)
            {
                dialogueindex++;
            }
            else
            {
                dialogueindex = 0;
            }
            dialogueindex = Mathf.Clamp(dialogueindex, 0, currentdialogueSO.DialogueArray.Length - 1);
            if (UIManager.Instance == null)
            {
                Debug.LogError("[DialogueManager] UIManager.Instance is null");
                _hasActiveDialogue = false;
                return;
            }
            
            UIManager.Instance.DisplayClueHUD(currentdialogueSO.DialogueArray[dialogueindex].Dialogue);
        }
        
        private void OnDialogueFinished()
        {
            _isTransitioning = true;
            _hasActiveDialogue = false;  
            if (GameManager.Instance == null)
            {
                Debug.LogError("[DialogueManager] GameManager.Instance is null during OnDialogueFinished");
                _isTransitioning = false;
                return;
            }
            if (GameManager.Instance.playerStateMachine == null)
            {
                Debug.LogError("[DialogueManager] playerStateMachine is null during OnDialogueFinished");
                _isTransitioning = false;
                return;
            }
            
            GameManager.Instance.playerStateMachine.changeState(GameManager.Instance.playerStateMachine.idlestate);
            Debug.Log("[DialogueManager] Dialogue finished - returning to idle state");
            if (EventManager.instance != null)
            {
                EventManager.instance.Publish(new DialogueFinishedEvent());
            }
            else
            {
                Debug.LogError("[DialogueManager] EventManager.instance is null");
            }
            
            _isTransitioning = false;
        }
    }
}