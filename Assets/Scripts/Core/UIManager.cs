using TMPro;
using UnityEngine;

namespace Core
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        [SerializeField] private GameObject toast;
        [SerializeField] private TMP_Text Toasttext;
        [SerializeField] private GameObject clueHUD;
        [SerializeField] private TMP_Text clueText;
        
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;
    
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }

        public void DisplayToast(string message)
        {
            Toasttext.text = message;
            toast.SetActive(true);
        }

        public void DisplayClueHUD(string message)
        {
            clueText.text = message;
            clueHUD.SetActive(true);
        }
    }
}
