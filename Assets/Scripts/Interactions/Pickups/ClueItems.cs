using System.Collections;
using System.Collections.Generic;
using Core;
using Gameplay;
using UnityEngine;

namespace Interactions.Pickups
{
    public class ClueItems : InteractableObjects,ICollectible
    {
        [SerializeField] private DialogueSO _cluedialogueSO;
        [SerializeField] private string itemName;
        
        [SerializeField] private Renderer _objectRenderer;
        [SerializeField] private ParticleSystem _clueParticleSystem;
        [SerializeField] private float _secondsToWait = 0.2f;
        private Coroutine currentRoutine;

        protected override void OnInteracted()
        {
            DialogueManager.Instance.SetSequentialDialogue(_cluedialogueSO, InteractableObjectTypes.Clues);
            InventoryManager.Instance.AddClueToInventory(this, interactableObjectType);
            OnCollectEffect();
        }
        
        private void Start()
        {
            _clueParticleSystem = GetComponentInChildren<ParticleSystem>();
        }
        
        
        public Sprite Icon { get; set; }

        public void OnCollectEffect()
        {
            if(currentRoutine != null )return;
            // TODO: ParticleSystem
            currentRoutine = StartCoroutine(CollectParticleSystem());
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
    }
}
