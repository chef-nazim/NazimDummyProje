using System;
using System.Collections.Generic;
using UnityEngine;

namespace NCG.template.EventBus
{
    public static class EventBus<T> where T : IEvent
    {
        private static HashSet<Action<T>> eventListeners = new HashSet<Action<T>>();

        public static void Subscriber(Action<T> listener)
        {
            if (listener != null)
                eventListeners.Add(listener);
        }

        public static void Unsubscribe(Action<T> listener)
        {
            if (listener != null)
                eventListeners.Remove(listener);
        }

        public static void Publish(T @event)
        {
            foreach (var listener in eventListeners)
            {
                if (listener != null)
                    listener(@event);

                if (eventListeners.Count == 0)
                {
                    Debug.LogWarning($"No listener found for event {@event.GetType().Name}");
                    break;
                }
            }
        }

        public static void ClearAll()
        {
            Debug.Log($"Clearing {typeof(T).Name} listeners");
            eventListeners.Clear();

            eventListeners = null;
            GC.Collect();
        }
    }
}