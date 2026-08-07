using System;
using System.Collections.Generic;
using Core;
using EventSystem;
using UnityEngine;

namespace Story
{
    public class StoryManager : Singleton<StoryManager>
    {
        private List<StoryMarker> _discoveredMarkers = new List<StoryMarker>();

        private void OnEnable()
        {
            EventManager.instance.Subscribe<StoryMarkerUnlockedEvent>(OnMarkerUnlocked);
        }

        private void OnDisable()
        {
            EventManager.instance.Unsubscribe<StoryMarkerUnlockedEvent>(OnMarkerUnlocked);
        }

        private void OnMarkerUnlocked(StoryMarkerUnlockedEvent markerEvent)
        {
            if (markerEvent.Marker != null)
            {
                _discoveredMarkers.Add(markerEvent.Marker);
                Debug.Log($"[StoryManager] Marker Unlocked: {markerEvent.Marker.name}. Total Unlocked: {_discoveredMarkers.Count}");
            }
        }

        public bool HasMarker(StoryMarker marker)
        {
            if (_discoveredMarkers.Contains(marker)) return true;
            else
            {
                return false;
            }
        }
    }
}