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
        [SerializeField] private DialogueSO keyDialogueSo;   

        [Header("Visuals")]
        [SerializeField] private Renderer objectRenderer;
        [SerializeField] private ParticleSystem keyParticleSystem;
        [SerializeField] private float secondsToWait = 0.2f;
        [SerializeField] private string keyName;
        [SerializeField] private AudioClip pickupSound;

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
            var firstLine = keyDialogueSo.DialogueArray[0];

            bool requirementsMet = !firstLine.HasRequirements ||
                                   DialogueManager.Instance.RequirementsMet(firstLine);

            if (!requirementsMet)
            {
                DialogueManager.Instance.SetSequentialDialogue(keyDialogueSo);
                return;
            }
            alreadyCollected = true;
            EventManager.instance.Publish(new PickupEvent(this));
            InventoryManager.Instance.EquippedItem = gameObject;

            if (AudioManager.Instance != null && pickupSound != null)
                AudioManager.Instance.PlaySound(pickupSound);
            OnCollectEffect();
            DialogueManager.Instance.ProduceMarkers(firstLine, true);
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
