using System;
using System.Collections.Generic;
using System.Reflection;

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
    }
}