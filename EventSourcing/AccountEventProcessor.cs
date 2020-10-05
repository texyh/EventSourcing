using EventSourcing.Domain.Core;
using EventSourcing.Domain.EventProcessor;
using EventSourcing.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing
{
    public class AccountEventProcessor<TId> : IProcessor<TId>
    {

        private IDictionary<string, Action<IDomainEvent<TId>>> _actions;

        public AccountEventProcessor()
        {
            _actions = new Dictionary<string, Action<IDomainEvent<TId>>>();
        }

        public void ProcessAsync(object @event, Type type)
        {
            _actions.TryGetValue(type.Name, out var handler);

            if(handler == null)
            {
                throw new Exception($"Handler for the type {type.Name} not found");
            }

            var idevent = Convert.ChangeType(@event, type) as IDomainEvent<TId>;

            handler(idevent);
        }

        public void RegisterApplier<TEvent>(Action<TEvent> applier) where TEvent : IDomainEvent<TId>
        {
            _actions.Add(typeof(TEvent).Name, (x) => applier((TEvent)x));
        }

    }
}
