using UnityEngine;

namespace Core
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
    
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;
    
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
