using System;
using System.Collections.Generic;
using System.Reflection;
using NCG.template._NCG.Core.BaseClass;

namespace NCG.template.EventBus
{
    public static class EventFinder
    {
        public static List<Type> FindAllEventTypes()
        {
            List<Type> eventTypes = new List<Type>();
            Type eventInterface = typeof(IEvent);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (eventInterface.IsAssignableFrom(type) && type != eventInterface)
                    {
                        eventTypes.Add(type);
                    }
                }
            }

            return eventTypes;
        }
        
        public static List<Type> FindAllBaseManagerTypes()
        {
            List<Type> eventTypes = new List<Type>();
            Type eventInterface = typeof(BaseManager);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (eventInterface.IsAssignableFrom(type) && type != eventInterface)
                    {
                        eventTypes.Add(type);
                    }
                }
            }

            return eventTypes;
        }
        public static List<Type> FindAllBaseControllerTypes()
        {
            List<Type> eventTypes = new List<Type>();
            Type eventInterface = typeof(BaseController);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (eventInterface.IsAssignableFrom(type) && type != eventInterface)
                    {
                        eventTypes.Add(type);
                    }
                }
            }

            return eventTypes;
        }
    }
}