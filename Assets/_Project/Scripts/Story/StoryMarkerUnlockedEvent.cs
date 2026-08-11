using EventSystem;

namespace Story
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