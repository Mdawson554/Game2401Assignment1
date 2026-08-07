using System.Collections;
using System.Collections.Generic;
using Core;
using EventSystem;
using Interactions;
using Story;
using UnityEngine;

namespace Interactions.Pickups
{
    public class ClueItems : BaseItem, ICollectible, IInteractable
    {
        [Header("Dialogue")]
        [SerializeField] private DialogueSO clueDialogueSO;      // shown when picked up
        [SerializeField] private DialogueSO lockedDialogueSO;    // shown when requirements not met

        [Header("Visuals")]
        [SerializeField] private string keyName;
        [SerializeField] private Renderer objectRenderer;
        [SerializeField] private ParticleSystem clueParticleSystem;
        [SerializeField] private float secondsToWait = 0.2f;
        [SerializeField] private AudioClip pickupSound;

        [Header("Requirements")]
        public bool HasRequirement;
        public List<StoryMarker> RequiredMarkers;

        [Header("Unlocks")]
        public List<StoryMarker> MarkersUnlocked;

        private Coroutine currentRoutine;
        private bool alreadyCollected = false;

        private void Start()
        {
            clueParticleSystem = GetComponentInChildren<ParticleSystem>();
            if (clueParticleSystem == null)
                Debug.LogWarning($"Clue '{keyName}': ParticleSystem not found");

            if (objectRenderer == null)
                Debug.LogError($"Clue '{keyName}': Renderer not assigned!");
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

            if (clueDialogueSO != null)
                DialogueManager.Instance.SetSequentialDialogue(clueDialogueSO);

            if (AudioManager.Instance != null && pickupSound != null)
                AudioManager.Instance.PlaySound(pickupSound);

            InventoryManager.Instance?.IncrementClueCount();

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
                Debug.LogWarning($"Clue '{keyName}': LockedDialogueSO not assigned!");
                return;
            }

            DialogueManager.Instance.SetSequentialDialogue(lockedDialogueSO);
        }

        public Sprite Icon { get; set; }

        public void OnCollectEffect()
        {
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            currentRoutine = StartCoroutine(CollectParticleSystem());
        }

        public override string GetItemName()
        {
            return keyName;
        }

        private IEnumerator CollectParticleSystem()
        {
            if (objectRenderer == null)
                yield break;

            Color previousColor = objectRenderer.material.color;

            if (clueParticleSystem != null)
            {
                var main = clueParticleSystem.main;
                main.startColor = previousColor;
                clueParticleSystem.Play();
            }

            objectRenderer.material.color = Color.black;
            yield return new WaitForSeconds(secondsToWait);

            objectRenderer.material.color = previousColor;
            yield return new WaitForSeconds(secondsToWait);

            objectRenderer.material.color = Color.black;

            Destroy(gameObject);
        }
    }
}