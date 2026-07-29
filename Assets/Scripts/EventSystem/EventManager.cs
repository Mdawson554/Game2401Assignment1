using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventSystem
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager instance;
        private readonly Dictionary<Type, Delegate> _subscribers = new Dictionary<Type, Delegate>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void Publish<T>(T eventData) where T : IEvent
        {
            Type type = typeof(T);
            if (_subscribers.TryGetValue(type, out Delegate del))
            {
                (del as Action<T>)?.Invoke(eventData);
            }
        }
    
        public void Subscribe<T>(Action<T> onEventTrigger) where T : IEvent
        {
            Type type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers[type] = null;
            }
            _subscribers[type] = (Action<T>)_subscribers[type] + onEventTrigger;
        }
    
        public void Unsubscribe<T>(Action<T> onEventTrigger) where T : IEvent
        {
            Type type = typeof(T);
            if (_subscribers.ContainsKey(type))
            {
                _subscribers[type] = (Action<T>)_subscribers[type] - onEventTrigger;
            }
        }
    }
}
