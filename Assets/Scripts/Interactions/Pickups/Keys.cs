using System.Collections;
using System.Collections.Generic;
using Core;
using EventSystem;
using Interactions;
using Story;
using UnityEngine;

namespace Pickups
{
    public class Keys : BaseItem, IInteractable, ICollectible
    {
        [SerializeField] private Renderer _objectRenderer;
        [SerializeField] private ParticleSystem _keyParticleSystem;
        [SerializeField] private float _secondsToWait = 0.2f;
        [SerializeField] private string keyName;
        [SerializeField] private AudioClip keypickup;
        
        [Header("Requirements")]
        public bool HasRequirement;
        public List<StoryMarker> RequiredMarkers;
        
        [Header("Unlocks")]
        public List<StoryMarker> MarkersUnlocked;
        
        [Header("Feedback")]
        [TextArea(2, 3)]
        public string LockedMessage = "I can't pick this up right now...";
        
        private bool _alreadyCollected = false;
        
        private void Start()
        {
            _keyParticleSystem = GetComponentInChildren<ParticleSystem>();
            if (_keyParticleSystem == null)
            {
                Debug.LogWarning($"Key '{keyName}': ParticleSystem not found as child component");
            }
            if (_objectRenderer == null)
            {
                Debug.LogError($"Key '{keyName}': Renderer is not assigned in inspector!");
            }
            EventManager.instance.Subscribe<StoryMarkerUnlockedEvent>(UnlockMarker);
        }
        
        private void OnDestroy()
        {
            if (EventManager.instance != null)
            {
                EventManager.instance.Unsubscribe<StoryMarkerUnlockedEvent>(UnlockMarker);
            }
        }
        
        private void OnInteracted()
        {
            if (_alreadyCollected)
            {
                Debug.LogWarning($"Key '{keyName}': Already collected. Ignoring duplicate interaction.");
                return;
            }
            if (HasRequirement)
            {
                if (RequiredMarkers.Count > 0)
                {
                    Debug.Log($"Key '{keyName}' is locked. Missing {RequiredMarkers.Count} marker(s)");
                    UIManager.Instance.DisplayToast(LockedMessage);
                    return;
                }
            }
            _alreadyCollected = true;
            if (InventoryManager.Instance == null)
            {
                Debug.LogError($"Key '{keyName}': InventoryManager.Instance is null");
                _alreadyCollected = false;
                return;
            }
            EventManager.instance.Publish(new PickupEvent(this));
            InventoryManager.Instance.EquippedItem = gameObject;
            if (AudioManager.Instance != null && keypickup != null)
            {
                AudioManager.Instance.PlaySound(keypickup);
            }
            else if (keypickup == null)
            {
                Debug.LogWarning($"Key '{keyName}': keypickup AudioClip is not assigned");
            }
            
            OnCollectEffect();
            if (EventManager.instance != null)
            {
                foreach (var marker in MarkersUnlocked)
                {
                    if (marker != null)
                    {
                        EventManager.instance.Publish(new StoryMarkerUnlockedEvent(marker));
                        Debug.Log($"Key '{keyName}' unlocked marker '{marker.name}'");
                    }
                    else
                    {
                        Debug.LogError($"Key '{keyName}': MarkersUnlocked contains null marker!");
                    }
                }
            }
        }
        
        public override string GetItemName()
        {
            return keyName;
        }
        
        public Sprite Icon { get; set; }
        
        public void OnCollectEffect()
        {
            StartCoroutine(CollectParticleSystem());
        }
        
        private IEnumerator CollectParticleSystem()
        {
            if (_objectRenderer == null)
            {
                Debug.LogError($"Key '{keyName}': Renderer is null during CollectParticleSystem");
                yield break;
            }
            
            Color previousColor = _objectRenderer.material.color;
            if (_keyParticleSystem != null)
            {
                var main = _keyParticleSystem.main;
                main.startColor = previousColor;
                _keyParticleSystem.Play();
            }
            
            _objectRenderer.material.color = Color.white;
            yield return new WaitForSeconds(_secondsToWait);
            if (_objectRenderer != null)
            {
                _objectRenderer.material.color = previousColor;
            }
            
            yield return new WaitForSeconds(_secondsToWait);
            if (_objectRenderer != null)
            {
                _objectRenderer.material.color = Color.white;
            }
            
            Destroy(gameObject);
        }
        
     
        private void UnlockMarker(StoryMarkerUnlockedEvent e)
        {
            if (e == null || e.Marker == null)
            {
                Debug.LogError($"Key '{keyName}': Received invalid StoryMarkerUnlockedEvent");
                return;
            }
            if (RequiredMarkers.Contains(e.Marker))
            {
                RequiredMarkers.Remove(e.Marker);
                Debug.Log($"Key '{keyName}' now has marker '{e.Marker.name}'. Remaining requirements: {RequiredMarkers.Count}");
                
                if (RequiredMarkers.Count == 0)
                {
                    Debug.Log($"Key '{keyName}' is now unlocked!");
                }
            }
        }
        
        public void OnInteract()
        {
            OnInteracted();
        }
    }
}