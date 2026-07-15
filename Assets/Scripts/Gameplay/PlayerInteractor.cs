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

/*using UnityEngine;
using Interactions;
using Core;

public class PlayerInteractor : MonoBehaviour
{
    public InteractableObjects Interactable;
    private InteractableObjects _tempInteractable;

    private void OnTriggerEnter(Collider other)
    {
        _tempInteractable = other.GetComponent<InteractableObjects>();

        if (_tempInteractable != null)
        {
            Interactable = _tempInteractable;
            Interactable.OnHoverIn();
            UIManager.Instance.ShowToastPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Interactable != null)
        {
            Interactable.OnHoverOff();
            Interactable = null;
            UIManager.Instance.HideToastPrompt();
        }
    }

    public void InteractWithObject()
    {
        if (Interactable == null) return;

        IConditional conditional = Interactable as IConditional;

        if (conditional != null)
        {
            if (!conditional.CanInteract())
            {
                conditional.OnConditionFailed();
                return;
            }
        }

        UIManager.Instance.HideToastPrompt();
        Interactable.OnInteract();
    }
}*/