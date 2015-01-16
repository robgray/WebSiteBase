using System;
using System.Collections.Generic;
using Castle.Windsor;

namespace Domain.Events
{
    public static class DomainEventDispatcher
    {
        [ThreadStatic]
        private static List<Delegate> _actions;

        private static IWindsorContainer Container { get; set; }

        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (_actions == null)
            {
                _actions = new List<Delegate>();
            }
            _actions.Add(callback);
        }
        
        public static void SetContainer(IWindsorContainer container)
        {
            Container = container;
        }
        
        public static void Raise<T>(T domainEvent) where T : class, IDomainEvent
        {
            if (Container != null)
            {
                foreach (var handler in Container.ResolveAll<IHandles<T>>())
                {
                    handler.Handle(domainEvent);
                }
            }

            if (_actions != null)
            {
                foreach (var action in _actions)
                    if (action is Action<T>)
                        ((Action<T>)action)(domainEvent);

            }

        }    
    }
}
