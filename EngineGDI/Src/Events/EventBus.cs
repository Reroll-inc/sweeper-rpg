using System;
using System.Collections.Generic;
using PlantUmlClassDiagramGenerator.Attributes;

namespace EngineGDI.Src.Events
{
    public class EventBus
    {
        [PlantUmlIgnoreAssociation]
        private readonly Dictionary<Type, List<object>> subscribers = [];

        public void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);

            if (!subscribers.TryGetValue(type, out List<object> value))
            {
                value = [];
                subscribers[type] = value;
            }

            value.Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);

            if (subscribers.TryGetValue(type, out List<object> value))
            {
                value.Remove(handler);
            }
        }

        public void Publish<T>(T eventData)
        {
            var type = typeof(T);

            if (subscribers.TryGetValue(type, out List<object> value))
            {
                foreach (var subscriber in value)
                {
                    ((Action<T>)subscriber)(eventData);
                }
            }
        }
    }
}
