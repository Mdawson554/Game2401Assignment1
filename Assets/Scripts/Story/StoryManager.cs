/*using System.Collections.Generic;
using EventSystem;
using Interactions;
using UnityEngine;

namespace Story
{
    public class StoryManager : MonoBehaviour
    {
        public static StoryManager Instance;

       
        public List<NPCs> NPCConditional;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            EventManager.instance.Subscribe<StoryMarkerUnlockedEvent>(UnlockMarker);
            foreach (var NPC in NPCConditional)
            {
                EventManager.instance.Subscribe<StoryMarkerUnlockedEvent>(UnlockMarker);
            }
        }
        
    }
}*/