using System.Collections;
using Core;
using EventSystem;
using Interactions;
using UnityEngine;

namespace Pickups
{
    public class Keys : BaseItem, IInteractable,ICollectible
    {
        [SerializeField] private Renderer _objectRenderer;
        [SerializeField]  private ParticleSystem _keyParticleSystem;
        [SerializeField] private float _secondsToWait = 0.2f;
        
        private void Start()
        {
            _keyParticleSystem = GetComponentInChildren<ParticleSystem>();
        }
        
        private void OnInteracted()
        {
            EventManager.instance.Publish(new PickupEvent(this));
            InventoryManager.Instance.EquippedItem = gameObject;
            OnCollectEffect();
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

        public void OnInteract()
        {
            OnInteracted();
        }
    }
}

