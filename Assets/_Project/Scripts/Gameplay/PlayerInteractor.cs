using _Project.Scripts.Core;
using _Project.Scripts.Interactions;
using UnityEngine;
using UnityEngine.WSA;

namespace _Project.Scripts.Gameplay
{
    public class PlayerInteractor : MonoBehaviour
    {
        private Toast _toast;
        public IInteractable CurrentInteractable;

        private void OnTriggerEnter(Collider other)
        {
            var interactableinterface = other.TryGetComponent(out IInteractable interactable);
            if (interactableinterface)
            {
                CurrentInteractable = interactable;
                UIManager.Instance.ShowToastPrompt();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            UIManager.Instance.HideToastPrompt();
        }
    }
}