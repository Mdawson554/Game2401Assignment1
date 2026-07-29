using System.Collections;
using TMPro;
using UnityEngine;

namespace Core
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private float itemCollectedDisplaytime;
        [SerializeField] private float dialogueDisplaytime;
        
        [Header("text")]
        [SerializeField] private TMP_Text toastText;
        [SerializeField] private TMP_Text clueText;
        
        [Header("panels")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject winMenu;
        [SerializeField] private GameObject clueHUD;
        [SerializeField] private GameObject toast;
        
        private string _defaultToastText;
        
        private GameManager gameManager;
        
        private void Start()
        {
            gameManager = GameManager.Instance;
            _defaultToastText = toastText.text;
        }

        public void DisplayToast(string message)
        {
            toast.SetActive(true);
            toastText.text = message;
        }

        public void DisplayClueHUD(string message)
        {
            StopAllCoroutines();
            clueHUD.SetActive(true);
            clueText.text = message;
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
