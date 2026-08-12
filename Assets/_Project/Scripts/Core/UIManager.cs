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
        
        private string _defaultToastText;
        private void Start()
        {
            _defaultToastText = toastText.text;
            _defaultToastText = toastText.text;
            _defaultToastColor = toastText.color;
            _defaultToastFont = toastText.font;
            _defaultToastMaterial = toastText.fontMaterial;
        }
        public void DisplayToast(DialogueStruct dialogue)
        {
            toast.SetActive(true);
            toastText.text = dialogue.Dialogue;
            toastText.color = dialogue.DialogueColor;
        }

        public void DisplayLockedToast(DialogueStruct dialogueStruct)
        {
            Debug.Log("locked");
            toast.SetActive(true);
            toastText.text = dialogueStruct.LockedDialogue;
            toastText.color = dialogueStruct.LockedDialogueColor;
            
        }
        
        public void DisplayClueHUD(DialogueStruct dialogue)
        {
            StopAllCoroutines();
            clueHUD.SetActive(true);
            clueText.text = dialogue.Dialogue;
            clueText.color = dialogue.DialogueColor;
            StartCoroutine(HideClueAfterDelay());
        }
        
        public void DisplayLockedClueHUD(DialogueStruct dialogueStruct)
        {
            Debug.Log("clue locked");
            StopAllCoroutines();
            clueHUD.SetActive(true);
            clueText.text = dialogueStruct.LockedDialogue;
            clueText.color = dialogueStruct.LockedDialogueColor;
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