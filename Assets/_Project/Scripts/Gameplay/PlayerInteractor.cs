using _Project.Scripts.Core;
using _Project.Scripts.Interactions;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    public class PlayerInteractor : MonoBehaviour
    {
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