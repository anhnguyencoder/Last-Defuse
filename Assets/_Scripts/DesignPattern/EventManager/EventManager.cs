using System;
using System.Collections.Generic;

namespace _Scripts.DesignPattern.EventManager
{
    public class EventManager : Singleton<EventManager>
    {
        private readonly Dictionary<Type, Delegate> _eventListeners = new();

        public void Subscribe<T>(Action<T> listener) where T : struct
        {
            Type eventType = typeof(T);
            if (_eventListeners.TryGetValue(eventType, out var exitingDelegate))
            {
                _eventListeners[eventType] = Delegate.Combine(exitingDelegate, listener);
            }
            else
            {
                _eventListeners[eventType] = listener;
            }
        }

        public void Unsubscribe<T>(Action<T> listener) where T : struct
        {
            Type eventType = typeof(T);
            if (_eventListeners.TryGetValue(eventType, out var existingDelegate))
            {
                _eventListeners[eventType] = Delegate.Remove(existingDelegate, listener);
            }
        }

        public void Publish<T>(T eventData) where T : struct
        {
            Type eventType = typeof(T);
            if (_eventListeners.TryGetValue(eventType, out var eventDelegate))
            {
                (eventDelegate as Action<T>)?.Invoke(eventData);
            }
        }
    }
}