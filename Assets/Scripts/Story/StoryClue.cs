/*using Core;
using EventSystem;
using Gameplay;
using Interactions;
using Story;
using UnityEngine;

public class StoryClue : StoryInteractable, IInteractable, ICollectible
{
    [Header("Clue")]
    [SerializeField] private DialogueSO clueDialogue;
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private ParticleSystem clueParticleSystem;
    [SerializeField] private float secondsToWait = 0.2f;
    [SerializeField] private string clueName;

    private Coroutine currentRoutine;

    public Sprite Icon { get; set; }

    public string GetItemName()
    {
        return clueName;
    }

    protected override void OnSuccessfulInteraction()
    {
        // Existing clue behaviour
        EventManager.instance.Publish(new PickupEvent(this));
        InventoryManager.Instance.IncrementClueCount();
        DialogueManager.Instance.SetSequentialDialogue(clueDialogue);
        OnCollectEffect();
    }

    public void OnCollectEffect()
    {
        if (currentRoutine != null)
            return;

        currentRoutine = StartCoroutine(CollectParticleSystem());
    }

    private System.Collections.IEnumerator CollectParticleSystem()
    {
        Color previousColor = objectRenderer.material.color;

        var main = clueParticleSystem.main;
        main.startColor = previousColor;

        clueParticleSystem.Play();

        objectRenderer.material.color = Color.black;

        yield return new WaitForSeconds(secondsToWait);

        objectRenderer.material.color = previousColor;

        yield return new WaitForSeconds(secondsToWait);

        objectRenderer.material.color = Color.black;

        Destroy(gameObject);
    }

    public void OnInteract()
    {
        Interact();
    }
}*/