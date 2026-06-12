using Core;
using Interactions;
using States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class PlayerInteractor : MonoBehaviour
{
    public InteractableObjects Interactable;
    private InteractableObjects _tempInteractable;
    private Toast _toast;

    private void OnTriggerEnter(Collider other)
    {
        _tempInteractable = other.GetComponent<InteractableObjects>();
        
        if (_tempInteractable != null)
        {
            Interactable = _tempInteractable;
            Interactable?.OnHoverIn();
            UIManager.Instance.ShowToastPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable?.OnHoverOff();
        Interactable = null;
        UIManager.Instance.HideToastPrompt();
    }
}
