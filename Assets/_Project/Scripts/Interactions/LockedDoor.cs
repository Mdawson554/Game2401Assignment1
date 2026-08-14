using _Project.Scripts.Core;
using _Project.Scripts.Interactions.Pickups;
using UnityEngine;

namespace _Project.Scripts.Interactions
{
    public class LockedDoor : MonoBehaviour, IConditional, IInteractable
    {
        [Header("Door Settings")]
        [SerializeField] private Animator anim;
        [SerializeField] private Keys requiredKey;
        [SerializeField] private AudioClip doorLocked;
        [SerializeField] private AudioClip doorOpen;
        [SerializeField] private AudioClip doorClose;

        [Header("Locked Message")]
        [SerializeField] private string lockedText = "The door is locked.";
        [SerializeField] private Color lockedTextColor = Color.red;

        private int isOpenHash;
        private bool _isOpen = false;

        private void Awake()
        {
            isOpenHash = Animator.StringToHash("isOpen");
        }

        public void OnKeyNotPickedUp()
        {
            AudioManager.Instance.PlaySound(doorLocked);
            UIManager.Instance.DisplayLockedMessage(lockedText, lockedTextColor);
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
                UIManager.Instance.DisplayLockedMessage(lockedText, lockedTextColor);
            }
        }

        public void OnDoorOpen()
        {
            if (_isOpen)
            {
                _isOpen = false;
                anim?.SetBool(isOpenHash, false);
                return;
            }
            _isOpen = true;
            if (anim != null)
                anim.SetBool(isOpenHash, true);
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
