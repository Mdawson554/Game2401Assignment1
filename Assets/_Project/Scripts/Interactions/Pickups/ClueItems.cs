using System.Collections;
using _Project.ScriptableObjects;
using _Project.Scripts.Core;
using _Project.Scripts.EventSystem;
using _Project.Scripts.Gameplay;
using _Project.Scripts.Tools;
using UnityEngine;

namespace _Project.Scripts.Interactions.Pickups
{
    public class ClueItems : BaseItem, ICollectible, IInteractable
    {
        [Header("Dialogue")]
        [SerializeField] private DialogueSO clueDialogueSO;   

        [Header("Visuals")]
        [SerializeField] private string keyName;
        [SerializeField] private Renderer objectRenderer;
        [SerializeField] private ParticleSystem clueParticleSystem;
        [SerializeField] private float secondsToWait = 0.2f;
        [SerializeField] private AudioClip pickupSound;

        private Coroutine currentRoutine;
        private bool alreadyCollected = false;

        private void Start()
        {
            clueParticleSystem = GetComponentInChildren<ParticleSystem>();
            if (clueParticleSystem == null)
                DevLogger.Log($"Clue '{keyName}': ParticleSystem not found");

            if (objectRenderer == null)
                DevLogger.Log($"Clue '{keyName}': Renderer not assigned!");
        }

        public void OnInteract()
        {
            OnInteracted();
        }

        private void OnInteracted()
        {
            if (alreadyCollected)
                return;
            DialogueStruct firstLine = clueDialogueSO.DialogueArray[0];
            bool requirementsMet =
                DialogueManager.Instance.RequirementsMet(firstLine);
            if (!requirementsMet)
            {
                DevLogger.Log("[CLUE] Requirement not met. Clue remains uncollected.");

                DialogueManager.Instance.SetSequentialDialogue(clueDialogueSO);
                return;
            }
            DevLogger.Log("[CLUE] Requirement met. Collecting clue.");
            alreadyCollected = true;
            EventManager.instance.Publish(new PickupEvent(this));
            DialogueManager.Instance.SetSequentialDialogue(clueDialogueSO);
            if (AudioManager.Instance != null && pickupSound != null)
                AudioManager.Instance.PlaySound(pickupSound);
            InventoryManager.Instance?.IncrementClueCount();
            OnCollectEffect();
            DialogueManager.Instance.ProduceMarkers(firstLine);
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
