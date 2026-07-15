using Core;
using Interactions;
using States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

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