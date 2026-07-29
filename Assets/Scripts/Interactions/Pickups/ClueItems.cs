using System.Collections;
using System.Collections.Generic;
using Core;
using EventSystem;
using Gameplay;
using Interactions;
using Story;
using UnityEngine;

namespace Interactions.Pickups
{
    public class ClueItems : BaseItem, ICollectible, IInteractable
    {
        [SerializeField] private DialogueSO _cluedialogueSO;
        [SerializeField] private string keyName;
        [SerializeField] private Renderer _objectRenderer;
        [SerializeField] private ParticleSystem _clueParticleSystem;
        [SerializeField] private float _secondsToWait = 0.2f;
        [SerializeField] private AudioClip pickupclue;
        
        [Header("Requirements")]
        public bool HasRequirement;
        public List<StoryMarker> RequiredMarkers;
        
        [Header("Unlocks")]
        public List<StoryMarker> MarkersUnlocked;
        
        [Header("Feedback")]
        [TextArea(2, 3)]
        public string LockedMessage = "I can't pick this up right now...";
        
        private Coroutine currentRoutine;
        private bool _alreadyCollected = false;
        
        private void Start()
        {
            _clueParticleSystem = GetComponentInChildren<ParticleSystem>();
            if (_clueParticleSystem == null)
            {
                Debug.LogWarning($"Clue '{keyName}': ParticleSystem not found as child component");
            }
            if (_objectRenderer == null)
            {
                Debug.LogError($"Clue '{keyName}': Renderer is not assigned in inspector!");
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
                Debug.LogWarning($"Clue '{keyName}': Already collected. Ignoring duplicate interaction.");
                return;
            }
            if (HasRequirement)
            {
                if (RequiredMarkers.Count > 0)
                {
                    Debug.Log($"Clue '{keyName}' is locked. Missing {RequiredMarkers.Count} marker(s)");
                    UIManager.Instance.DisplayToast(LockedMessage);
                    return;
                }
            }
            _alreadyCollected = true;
            if (EventManager.instance == null)
            {
                Debug.LogError($"Clue '{keyName}': EventManager.instance is null");
                _alreadyCollected = false;
                return;
            }
            EventManager.instance.Publish(new PickupEvent(this));
            if (DialogueManager.Instance == null)
            {
                Debug.LogError($"Clue '{keyName}': DialogueManager.Instance is null");
                _alreadyCollected = false;
                return;
            }
            if (_cluedialogueSO == null)
            {
                Debug.LogError($"Clue '{keyName}': _cluedialogueSO is not assigned!");
                _alreadyCollected = false;
                return;
            }
            
            DialogueManager.Instance.SetSequentialDialogue(_cluedialogueSO);
            if (AudioManager.Instance != null && pickupclue != null)
            {
                AudioManager.Instance.PlaySound(pickupclue);
            }
            else if (pickupclue == null)
            {
                Debug.LogWarning($"Clue '{keyName}': pickupclue AudioClip is not assigned");
            }
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.IncrementClueCount();
            }
            else
            {
                Debug.LogError($"Clue '{keyName}': InventoryManager.Instance is null");
            }
            OnCollectEffect();
            foreach (var marker in MarkersUnlocked)
            {
                if (marker != null)
                {
                    EventManager.instance.Publish(new StoryMarkerUnlockedEvent(marker));
                    Debug.Log($"Clue '{keyName}' unlocked marker '{marker.name}'");
                }
                else
                {
                    Debug.LogError($"Clue '{keyName}': MarkersUnlocked contains null marker!");
                }
            }
        }
        
        public Sprite Icon { get; set; }
        
        public void OnCollectEffect()
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
            }
            currentRoutine = StartCoroutine(CollectParticleSystem());
        }
        
        public override string GetItemName()
        {
            return keyName;
        }
        
        private IEnumerator CollectParticleSystem()
        {
            if (_objectRenderer == null)
            {
                Debug.LogError($"Clue '{keyName}': Renderer is null during CollectParticleSystem");
                yield break;
            }
            
            Color previousColor = _objectRenderer.material.color;
            if (_clueParticleSystem != null)
            {
                var main = _clueParticleSystem.main;
                main.startColor = previousColor;
                _clueParticleSystem.Play();
            }
            
            _objectRenderer.material.color = Color.black;
            yield return new WaitForSeconds(_secondsToWait);
            if (_objectRenderer != null)
            {
                _objectRenderer.material.color = previousColor;
            }
            
            yield return new WaitForSeconds(_secondsToWait);
            if (_objectRenderer != null)
            {
                _objectRenderer.material.color = Color.black;
            }
            
            Destroy(gameObject);
        }
        private void UnlockMarker(StoryMarkerUnlockedEvent e)
        {
            if (e == null || e.Marker == null)
            {
                Debug.LogError($"Clue '{keyName}': Received invalid StoryMarkerUnlockedEvent");
                return;
            }
            if (RequiredMarkers.Contains(e.Marker))
            {
                RequiredMarkers.Remove(e.Marker);
                Debug.Log($"Clue '{keyName}' now has marker '{e.Marker.name}'. Remaining requirements: {RequiredMarkers.Count}");
                
                if (RequiredMarkers.Count == 0)
                {
                    Debug.Log($"Clue '{keyName}' is now unlocked!");
                }
            }
        }
        
        public void OnInteract()
        {
            OnInteracted();
        }
    }
}