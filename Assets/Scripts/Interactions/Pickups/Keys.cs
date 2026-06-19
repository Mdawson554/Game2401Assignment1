using System;
using System.Collections;
using Core;
using UnityEngine;

namespace Interactions.Pickups
{
    public class Keys : InteractableObjects,ICollectible
    {
        public int KeyValue;
        
        //effects for pickup
        [SerializeField] private Renderer _objectRenderer;
        [SerializeField]  private ParticleSystem _keyParticleSystem;
        [SerializeField] private float _secondsToWait = 0.2f;

        
        protected override void OnInteracted()
        {
            OnCollectEffect();
        }

        private void Start()
        {
            _keyParticleSystem = GetComponentInChildren<ParticleSystem>();
        }

        protected override void OnFullfiledRequirements()
        {
            InventoryManager.Instance.UnequipItem();
            Debug.Log("Key used and unequipped");
        }
        
        
        public void OnCollectEffect()
        {
            // TODO: ParticleSystem
            Debug.Log("ClueItem particle effect");
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
