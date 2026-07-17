using System.Collections;
using System.Collections.Generic;
using Core;
using EventSystem;
using Gameplay;
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
        private Coroutine currentRoutine;


        private void OnInteracted()
        {
            EventManager.instance.Publish(new PickupEvent(this));
            DialogueManager.Instance.SetSequentialDialogue(_cluedialogueSO);
            InventoryManager.Instance.IncrementClueCount();
            OnCollectEffect();
        }

        private void Start()
        {
            _clueParticleSystem = GetComponentInChildren<ParticleSystem>();
        }
        
        public Sprite Icon { get; set; }

        public void OnCollectEffect()
        {
            if (currentRoutine != null) return;
            currentRoutine = StartCoroutine(CollectParticleSystem());
        }
        
        public override string GetItemName()
        {
            return keyName;
        }


        private IEnumerator CollectParticleSystem()
        {
            Color previousColor = _objectRenderer.material.color;
            var main = _clueParticleSystem.main;
            main.startColor = previousColor;
            _clueParticleSystem.Play();
            _objectRenderer.material.color = Color.black;
            yield return new WaitForSeconds(_secondsToWait);
            _objectRenderer.material.color = previousColor;
            yield return new WaitForSeconds(_secondsToWait);
            _objectRenderer.material.color = Color.black;
            Destroy(gameObject);
            yield return null;
        }

        public void OnInteract()
        {
            OnInteracted();
        }
    }
}
