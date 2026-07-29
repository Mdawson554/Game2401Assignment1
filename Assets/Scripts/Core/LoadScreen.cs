using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class LoadScreen : MonoBehaviour
    {
        public string sceneName;
        [SerializeField] private Button playGameButton;
        [SerializeField] private Button quitButton;

        private void OnEnable()
        {
            if (playGameButton == null || quitButton == null) return;
            playGameButton.onClick.AddListener(OnButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnDisable()
        {
            if (playGameButton == null || quitButton == null) return;
            playGameButton.onClick.RemoveListener(OnButtonClicked);
            quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }

        private void OnButtonClicked()
        {
            SceneManager.LoadScene(sceneName);
        }

        private void OnQuitButtonClicked()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
