using System;
using System.Collections;
using Core;
using UnityEngine;

namespace Interactions.Pickups
{
    public class Keys : InteractableObjects,ICollectible
    {
        [SerializeField] private Renderer _objectRenderer;
        [SerializeField]  private ParticleSystem _keyParticleSystem;
        [SerializeField] private float _secondsToWait = 0.2f;
        public int KeyValue;
        
        
        private void Start()
        {
            _keyParticleSystem = GetComponentInChildren<ParticleSystem>();
        }
        
        protected override void OnInteracted()
        {
            OnCollectEffect();
           //InventoryManager.Instance.AddKeyToInventory(this, interactableObjectType);
        }
        
        public Sprite Icon { get; set; }

        public void OnCollectEffect()
        {
            StartCoroutine(CollectParticleSystem());
        }
        
        private IEnumerator CollectParticleSystem()
        {
            //the coroutine for the particle system when the collectible is collided with 
            Color previousColor = _objectRenderer.material.color;
            var main = _keyParticleSystem.main;
            main.startColor = previousColor;
            _keyParticleSystem.Play();
            _objectRenderer.material.color = Color.white; 
            yield return new WaitForSeconds(_secondsToWait); 
            _objectRenderer.material.color = previousColor; 
            yield return new WaitForSeconds(_secondsToWait);
            _objectRenderer.material.color = Color.white; 
            Destroy(gameObject);
        }
    }
}
