using UnityEngine;
using Interactions;
using Core;
using Interactions.Pickups;
using Pickups;
using UnityEngine.InputSystem;

public class LockedDoor : MonoBehaviour, IConditional, IInteractable
{
    [SerializeField] private Animator anim;
    [SerializeField] private Keys requiredKey;
    [SerializeField] private AudioClip doorLocked;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;
    private int isOpenHash;
    private bool _isOpen = false;
    
    public void OnKeyNotPickedUp()
    {
        AudioManager.Instance.PlaySound(doorLocked);
    }
    
    public void OnTryOpen()
    {
        var items = InventoryManager.Instance.Keys;
        Debug.Log(items.Count);
        if (items.ContainsValue(requiredKey.KeyValue))
        {
            AudioManager.Instance.PlaySound(doorOpen);
            Debug.Log("open");
            OnDoorOpen();
        }
        else
        {
            AudioManager.Instance.PlaySound(doorClose);
            Debug.Log("NUhUh");
        }
    }

    public void OnDoorOpen()
    {
        if (_isOpen)
        {
            if (anim != null)
            {
                //anim.SetBool(isOpenHash, false);
            }
            _isOpen = false;
            return;
        }
        // OPEN DOOR
        _isOpen = true;
        Debug.Log("door open");
        if (anim != null)
        {
            //anim.SetBool(isOpenHash, true);
        }
    }

    private void OnInteracted()
    {
        OnTryOpen();
    }

    public void OnInteract()
    {
        OnInteracted();
    }
}