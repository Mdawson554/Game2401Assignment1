using TMPro;
using UnityEngine;

namespace Core
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        [SerializeField] private GameObject toast;
        [SerializeField] private TMP_Text text;
        
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
            text.text = message;
            toast.SetActive(true);
        }
    }
}
