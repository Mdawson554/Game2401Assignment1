using _Project.Scripts.Core;
using _Project.Scripts.Interactions.Pickups;
using UnityEngine;

namespace _Project.Scripts.Interactions
{
    public class LockedDoor : MonoBehaviour, IConditional, IInteractable
    {
        [SerializeField] private Animator anim;
        [SerializeField] private Keys requiredKey;
        [SerializeField] private AudioClip doorLocked;
        [SerializeField] private AudioClip doorOpen;
        [SerializeField] private AudioClip doorClose;
    
        private int isOpenHash;
        private bool _isOpen = false;
    
        private void Awake()
        {
            isOpenHash = Animator.StringToHash("isOpen");
        }
    
        public void OnKeyNotPickedUp()
        {
            AudioManager.Instance.PlaySound(doorLocked);
        }
    
        public void OnTryOpen()
        {
            var items = InventoryManager.Instance.Keys;
            if (items.ContainsValue(requiredKey.KeyValue))
            {
                AudioManager.Instance.PlaySound(doorOpen);
                OnDoorOpen();
            }
            else
            {
                AudioManager.Instance.PlaySound(doorLocked);
                Debug.Log("Player doesn't have the key");
            }
        }
    
        public void OnDoorOpen()
        {
            if (_isOpen)
            {
                _isOpen = false;
                if (anim != null)
                {
                    anim.SetBool(isOpenHash, false);
                }
                return;
            }
        
            // Open the door
            _isOpen = true;
            Debug.Log("door open");
            if (anim != null)
            {
                anim.SetBool(isOpenHash, true);
            }
            else
            {
                Debug.LogError("Animator is not assigned to LockedDoor!");
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
}