using System;
using NCG.template._NCG.Core.BaseClass;
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
       // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializAllBaseManager()
        {
            var eventTypes = EventFinder.FindAllBaseManagerTypes();
            foreach (var eventType in eventTypes)
            {
                var initializable = Activator.CreateInstance(eventType) as BaseManager;
                initializable?.Initialize();
                Debug.Log($"BaseManager = > {eventType.Name}  automatically Initialized");
            }
        }
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializAllBaseController()
        {
            var eventTypes = EventFinder.FindAllBaseControllerTypes();
            foreach (var eventType in eventTypes)
            {
                var initializable = Activator.CreateInstance(eventType) as BaseController;
                initializable?.Initialize();
                Debug.Log($"BaseController = > {eventType.Name}  automatically Initialized");
            }
        }
    }
}