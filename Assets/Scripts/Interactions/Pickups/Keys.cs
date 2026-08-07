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
        [Header("Dialogue")]
        [SerializeField] private DialogueSO lockedDialogueSO;   // shown when requirements not met

        [Header("Visuals")]
        [SerializeField] private Renderer objectRenderer;
        [SerializeField] private ParticleSystem keyParticleSystem;
        [SerializeField] private float secondsToWait = 0.2f;
        [SerializeField] private string keyName;
        [SerializeField] private AudioClip pickupSound;

        [Header("Requirements")]
        public bool HasRequirement;
        public List<StoryMarker> RequiredMarkers;

        [Header("Unlocks")]
        public List<StoryMarker> MarkersUnlocked;

        private bool alreadyCollected = false;

        private void Start()
        {
            keyParticleSystem = GetComponentInChildren<ParticleSystem>();
            if (keyParticleSystem == null)
                Debug.LogWarning($"Key '{keyName}': ParticleSystem not found");

            if (objectRenderer == null)
                Debug.LogError($"Key '{keyName}': Renderer not assigned!");
        }

        public void OnInteract()
        {
            OnInteracted();
        }

        private void OnInteracted()
        {
            if (alreadyCollected)
                return;

            // DYNAMIC REQUIREMENT CHECK
            bool requirementsMet = true;
            if (HasRequirement)
            {
                foreach (var marker in RequiredMarkers)
                {
                    if (marker != null && !StoryManager.Instance.HasMarker(marker))
                    {
                        requirementsMet = false;
                        break;
                    }
                }
            }

            // LOCKED PATH
            if (!requirementsMet)
            {
                PlayLockedDialogue();
                return;
            }

            // UNLOCKED PATH
            alreadyCollected = true;

            EventManager.instance.Publish(new PickupEvent(this));
            InventoryManager.Instance.EquippedItem = gameObject;

            if (AudioManager.Instance != null && pickupSound != null)
                AudioManager.Instance.PlaySound(pickupSound);

            OnCollectEffect();

            foreach (var marker in MarkersUnlocked)
            {
                if (marker != null && !StoryManager.Instance.HasMarker(marker))
                    EventManager.instance.Publish(new StoryMarkerUnlockedEvent(marker));
            }
        }

        private void PlayLockedDialogue()
        {
            if (lockedDialogueSO == null)
            {
                Debug.LogWarning($"Key '{keyName}': LockedDialogueSO not assigned!");
                return;
            }

            DialogueManager.Instance.SetSequentialDialogue(lockedDialogueSO);
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
            if (objectRenderer == null)
                yield break;

            Color previousColor = objectRenderer.material.color;

            if (keyParticleSystem != null)
            {
                var main = keyParticleSystem.main;
                main.startColor = previousColor;
                keyParticleSystem.Play();
            }

            objectRenderer.material.color = Color.white;
            yield return new WaitForSeconds(secondsToWait);

            objectRenderer.material.color = previousColor;
            yield return new WaitForSeconds(secondsToWait);

            objectRenderer.material.color = Color.white;

            Destroy(gameObject);
        }
    }
}