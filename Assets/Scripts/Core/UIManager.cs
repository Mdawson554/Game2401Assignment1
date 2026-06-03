using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        [SerializeField] private float _itemCollectedDisplaytime;
        [SerializeField] private float _DialogueDisplaytime;
        
        [SerializeField] private TMP_Text _toastText;
        [SerializeField] private TMP_Text _clueText;
        
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _clueHUD;
        [SerializeField] private GameObject _toast;
        
        private string _defaultToastText;
        
        private GameManager gameManager;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }
        
        private void Start()
        {
            gameManager = GameManager.Instance;
            _defaultToastText = _toastText.text;
        }

        public void DisplayToast(string message)
        {
            _toast.SetActive(true);
            _toastText.text = message;
        }

        public void DisplayClueHUD(string message)
        {
            StopAllCoroutines();
            _clueHUD.SetActive(true);
            _clueText.text = message;
            StartCoroutine(HideClueAfterDelay());
        }

        public IEnumerator HideClueAfterDelay()
        {
               yield return new WaitForSeconds(_itemCollectedDisplaytime);
               _clueHUD.SetActive(false);
        }

        public void hideDialogue()
        {
            _toast.SetActive(false);
        }
        
        public void ShowToastPrompt()
        {
            _toast.SetActive(true);
        }
        
        public void HideToastPrompt()
        {
            _toast.SetActive(false);
            ResetToastText();
        }

        public void ResetToastText()
        {
            _toastText.text = _defaultToastText;
        }
        public void ShowPauseMenu(bool show)
        {
            _pauseMenu.SetActive(show);
        }
    }
}
