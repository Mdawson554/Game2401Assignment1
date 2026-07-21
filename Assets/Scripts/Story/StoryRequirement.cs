using System;
using EventSystem;
using UnityEngine;

namespace Story
{
    [Serializable]
    public class StoryRequirement: IEvent
    {
        public StoryMarker Marker;

        [TextArea]
        public string LockedMonologue;
    }
}