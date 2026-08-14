using System.Collections;
using _Project.ScriptableObjects;
using _Project.Scripts.Core;
using _Project.Scripts.EventSystem;
using _Project.Scripts.Gameplay;
using UnityEngine;

namespace _Project.Scripts.Interactions.Pickups
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

            DialogueStruct firstLine = keyDialogueSo.DialogueArray[0];

            bool requirementsMet =
                DialogueManager.Instance.RequirementsMet(firstLine);

            if (!requirementsMet)
            {
                Debug.Log("[KEY] Requirement not met. Key remains uncollected.");

                DialogueManager.Instance.SetSequentialDialogue(keyDialogueSo);
                return;
            }

            Debug.Log("[KEY] Requirement met. Collecting key.");

            alreadyCollected = true;

            EventManager.instance.Publish(new PickupEvent(this));

            InventoryManager.Instance.EquippedItem = gameObject;

            if (AudioManager.Instance != null && pickupSound != null)
                AudioManager.Instance.PlaySound(pickupSound);
            OnCollectEffect();
            DialogueManager.Instance.SetSequentialDialogue(keyDialogueSo);
            DialogueManager.Instance.ProduceMarkers(firstLine);
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
