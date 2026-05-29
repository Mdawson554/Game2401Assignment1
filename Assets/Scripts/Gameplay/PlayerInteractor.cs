using Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private InputAction interactionInput;

    private InteractableObjects _interactable;
    private InteractableObjects _tempInteractable;

    private void OnEnable()
    {
        interactionInput.Enable();
        interactionInput.performed += Interact;
    }

    private void OnDisable()
    {
        interactionInput.Disable();
        interactionInput.performed -= Interact;
    }

    private void OnTriggerEnter(Collider other)
    {
        _tempInteractable = other.GetComponent<InteractableObjects>();

        if (_tempInteractable != null)
        {
            _interactable = _tempInteractable;
            _interactable?.OnHoverIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _interactable?.OnHoverOff();
        _interactable = null;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");
        _interactable?.OnInteract();
    }
}
