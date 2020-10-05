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
    public class AccountEventProcessor : IProcessor
    {

        private IDictionary<string, IDomainEventHandler<object>> Handlers { get; }


        public AccountEventProcessor(IDictionary<string, IDomainEventHandler<object>> handlers)
        {
            Handlers = handlers;
        }

        public async Task ProcessAsync(object @event, Type type)
        {
            var n = type.Name;
            Handlers.TryGetValue(type.Name, out var handler);

            if(handler == null)
            {
                throw new Exception($"Handler for the type {type.Name} not found");
            }

            await handler.HandleAsync((IDomainEvent<object>)@event);
        }
    }
}
