using System;
using UnityEngine;

namespace NCG.template.EventBus
{
    public static class EventLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadEvents()
        {
            var eventTypes = EventFinder.FindAllEventTypes();
            foreach (var eventType in eventTypes)
            {
                Type eventBusType = typeof(EventBus<>).MakeGenericType(eventType);
                Debug.Log($"EventBus<{eventType.Name}> automatically loaded.");
            }
        }
    }
}