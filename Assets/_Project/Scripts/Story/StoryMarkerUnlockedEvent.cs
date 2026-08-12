using _Project.Scripts.EventSystem;

namespace _Project.Scripts.Story
{
    public class StoryMarkerUnlockedEvent : IEvent
    {
        public StoryMarker Marker;

        public StoryMarkerUnlockedEvent(StoryMarker marker)
        {
            Marker = marker;
        }
    }
}