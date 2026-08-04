using System.Collections.Generic;
using Core;
using EventSystem;
using UnityEngine;

namespace Story
{
    public class StoryManager : Singleton<StoryManager>
    {
        private HashSet<StoryMarker> _unlockedMarkers = new HashSet<StoryMarker>();

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
                _unlockedMarkers.Add(markerEvent.Marker);
                Debug.Log($"[StoryManager] Marker Unlocked: {markerEvent.Marker.name}. Total Unlocked: {_unlockedMarkers.Count}");
            }
        }

        public bool HasMarker(StoryMarker marker)
        {
            return _unlockedMarkers.Contains(marker);
        }

        public IReadOnlyCollection<StoryMarker> GetUnlockedMarkers()
        {
            return _unlockedMarkers;
        }

        public void UnlockMarker(StoryMarker marker)
        {
            if (marker != null && _unlockedMarkers.Add(marker))
            {
                Debug.Log($"[StoryManager] Manually Unlocked: {marker.name}");
            }
        }

        public void ResetAllMarkers()
        {
            _unlockedMarkers.Clear();
            Debug.Log("[StoryManager] All markers reset");
        }
    }
}