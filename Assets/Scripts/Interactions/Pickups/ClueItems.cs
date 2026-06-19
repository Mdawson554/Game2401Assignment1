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
        private ParticleSystem _clueParticleSystem;
        [SerializeField] private float _secondsToWait = 0.2f;

        protected override void OnInteracted()
        {
            DialogueManager.Instance.SetSequentialDialogue(_cluedialogueSO, InteractableObjectTypes.Clues);
            InventoryManager.Instance.AddItemToInventory(this, itemName);
            OnCollectEffect();
            Debug.Log("dialogue from item"); 
            
        }
        
        protected override void OnFullfiledRequirements()
        {
            //logic for when I've used my key and stuff
            //probably indicate to the player that they key is only used for the one room.
            //signal to UI manager and audio manager
            //maybe make an event
            Debug.Log("clues FullfiledRequirements");
        }
        
        public void OnCollectEffect()
        {
            // TODO: ParticleSystem
            Debug.Log("ClueItem particle effect placeholder");
            StartCoroutine(CollectParticleSystem());
        }
        
        
        private IEnumerator CollectParticleSystem()
        {
            //the coroutine for the particle system when the collectible is collided with 
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
        }
    }
}
