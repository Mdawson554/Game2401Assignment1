using UnityEngine;
using Interactions;
using Core;

public class LockedDoor : InteractableObjects, IConditional
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject requiredKey;
    [SerializeField] private AudioClip doorLocked;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;

    private AudioManager audioManager;
    private int isOpenHash;
    private bool _isOpen = false;

    private void Start()
    {
        audioManager = AudioManager.Instance;

        /*
        if (anim != null)
        {
            isOpenHash = Animator.StringToHash("IsOpen");
        }*/
    }

    public bool CanInteract()
    {
        if (_isOpen) return true;
        // Player must have the correct key equipped
        return InventoryManager.Instance.EquippedItem == requiredKey;
    }

    public void OnKeyNotPickedUp()
    {
        audioManager.PlaySound(doorLocked);
    }

    public void OnHoverIn()
    {
        if (!_isOpen)
        {
            UIManager.Instance.ShowToastPrompt();
        }
    }

    public void OnHoverOff()
    {
        UIManager.Instance.HideToastPrompt();
    }

    protected override void OnInteracted()
    {
        if (_isOpen)
        {
            if (anim != null)
            {
                anim.SetBool(isOpenHash, false);
            }
            audioManager.PlaySound(doorClose);
            _isOpen = false;
            return;
        }

        // OPEN DOOR
        _isOpen = true;
        Debug.Log("door open");
        if (anim != null)
        {
            anim.SetBool(isOpenHash, true);
        }
        audioManager.PlaySound(doorOpen);
    }
}