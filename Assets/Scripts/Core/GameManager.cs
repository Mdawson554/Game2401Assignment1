using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;
        
        private AudioManager _audioManager;
        private UIManager _uiManager;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }
        
        private void Start()
        {
            Time.timeScale = 1;
            _audioManager = AudioManager.Instance;
            _uiManager = UIManager.Instance;
            ShowMouse(false);
            _audioManager.PlayBGMusic();
            _audioManager.PlayAmbientAudio();
        }
        
        private void OnEnable()
        {
            resumeButton.onClick.AddListener(OnResume);
            quitButton.onClick.AddListener(OnQuit);
        }

        private void OnDisable()
        {
            resumeButton.onClick.RemoveListener(OnResume);
            quitButton.onClick.RemoveListener(OnQuit);
        }

        private void ShowMouse(bool value)
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        public void Pause()
        {
            ShowMouse(true);
            Time.timeScale = 0;
            UIManager.Instance.ShowPauseMenu(true);
        }

        private void OnResume()
        {
            ShowMouse(false);
            Time.timeScale = 1;
            UIManager.Instance.ShowPauseMenu(false);
        }

        private void OnQuit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private void IncrementClueCount()
        {
            //add this to the inventory system to keep track of the players progress
        }

        private void IncrementkeyCount()
        {
            //add this to the inventory system to keep track of the players progress  
        }

        private void SetClueCount()
        {
           //possibly add this to a JSON save system like term one 
        }
        
        private void SetKeyCount()
        {
            //possibly add this to a JSON save system like term one  
        }
    }
}
