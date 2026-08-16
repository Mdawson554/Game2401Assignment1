using System.Collections;
using _Project.Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private float itemCollectedDisplaytime;
        [SerializeField] private float dialogueDisplaytime;
        [Header("text")]
        [SerializeField] private TMP_Text toastText;
        [SerializeField] private TMP_Text clueText;
        private Color _defaultToastColor;
        private TMP_FontAsset _defaultToastFont;
        private Material _defaultToastMaterial;
        [Header("panels")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject winMenu;
        [SerializeField] private GameObject clueHUD;
        [SerializeField] private GameObject toast;
        
        [Header("Dialogue Progression Indicator")]
        [SerializeField] private GameObject nectIcon;
        
        private string _defaultToastText;
        
        private void Start()
        {
            _defaultToastText = toastText.text;
            _defaultToastText = toastText.text;
            _defaultToastColor = toastText.color;
            _defaultToastFont = toastText.font;
            _defaultToastMaterial = toastText.fontMaterial;
            
            // Ensure indicator starts inactive
            if (nectIcon != null)
                nectIcon.SetActive(false);
        }
        
        public void DisplayToast(DialogueStruct dialogue, bool isSequentialDialogue = false)
        {
            toast.SetActive(true);
            toastText.text = dialogue.Dialogue;
            toastText.color = dialogue.DialogueColor;
            
            // Show indicator only for sequential dialogue
            if (nectIcon != null)
                nectIcon.SetActive(isSequentialDialogue);
        }

        public void DisplayLockedToast(DialogueStruct dialogueStruct)
        {
            toast.SetActive(true);
            toastText.text = dialogueStruct.LockedDialogue;
            toastText.color = dialogueStruct.LockedDialogueColor;
            
            // Hide indicator for locked dialogue
            if (nectIcon != null)
                nectIcon.SetActive(false);
        }
        
        public void DisplayClueHUD(DialogueStruct dialogue)
        {
            StopAllCoroutines();
            clueHUD.SetActive(true);
            clueText.text = dialogue.Dialogue;
            clueText.color = dialogue.DialogueColor;
            
            // Hide indicator for item dialogue
            if (nectIcon != null)
                nectIcon.SetActive(false);
            
            StartCoroutine(HideClueAfterDelay());
        }
        
        public void DisplayLockedClueHUD(DialogueStruct dialogueStruct)
        {
            StopAllCoroutines();
            clueHUD.SetActive(true);
            clueText.text = dialogueStruct.LockedDialogue;
            clueText.color = dialogueStruct.LockedDialogueColor;
            
            // Hide indicator for locked item dialogue
            if (nectIcon != null)
                nectIcon.SetActive(false);
            
            StartCoroutine(HideClueAfterDelay());
        }
        
        private IEnumerator HideClueAfterDelay()
        {
            yield return new WaitForSeconds(itemCollectedDisplaytime);
            clueHUD.SetActive(false);
        }
        
        public void ShowToastPrompt()
        {
            toast.SetActive(true);
        }
        
        public void HideToastPrompt()
        {
            toast.SetActive(false);
            
            // Hide indicator when toast is hidden
            if (nectIcon != null)
                nectIcon.SetActive(false);
            
            ResetToastText();
        }
        
        private void ResetToastText()
        {
            toastText.text = _defaultToastText;
            toastText.color = _defaultToastColor;
            toastText.font = _defaultToastFont;
            toastText.fontMaterial = _defaultToastMaterial;
            var c = toastText.color;
            c.a = 1f;
            toastText.color = c;
        }
        
        public void DisplayLockedMessage(string text, Color color)
        {
            toast.SetActive(true);
            toastText.text = text;
            toastText.color = color;
            
            // Hide indicator for locked messages
            if (nectIcon != null)
                nectIcon.SetActive(false);
        }

        public void ShowPauseMenu(bool show)
        {
            pauseMenu.SetActive(show);
        }
        
        public void ShowWinMenu(bool show)
        {
            winMenu.SetActive(show);
        }
    }
}