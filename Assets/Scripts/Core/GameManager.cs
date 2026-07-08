using States;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public string SceneName;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button continueButton;
        
        
        private AudioManager _audioManager;
        private UIManager _uiManager;
        public PlayerStateMachine playerStateMachine;
        
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
            quitButton.onClick.AddListener(OnQuit);
            resumeButton.onClick.AddListener(OnResume);
            continueButton.onClick.AddListener(OnContinue);
            
            playerStateMachine = FindFirstObjectByType<PlayerStateMachine>();
        }
        
        private void OnEnable()
        {
            resumeButton.onClick.AddListener(OnResume);
            continueButton.onClick.AddListener(OnContinue);
            quitButton.onClick.AddListener(OnQuit);
        }

        private void OnDisable()
        {
            resumeButton.onClick.RemoveListener(OnResume);
            continueButton.onClick.AddListener(OnContinue);
            quitButton.onClick.RemoveListener(OnQuit);
        }

        public void OnAllCluesCollected()
        {
            Debug.Log("YOU WIN THE GAME!");
            ShowMouse(true);
            Time.timeScale = 0;
            UIManager.Instance.ShowWinMenu(true);
        }

        private void ShowMouse(bool value)
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        public void Pause()
        {
            Debug.Log("Pause");
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
        
        private void OnContinue()
        {
            SceneManager.LoadScene(SceneName);
        }

        private void OnQuit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
