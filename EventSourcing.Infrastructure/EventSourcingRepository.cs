using System.Threading.Tasks;
using System.Reflection;
using System;
using EventSourcing.Domain.Core;
using EventSourcing.Domain.DataAccess.EventStore;
using EventSourcing.Domain.DataAccess;
using EventSourcing.Domain.EventProcessor;

namespace EventSourcing.Infrastructure
{
    public class EventSourcingRepository<TAggregate, TAggregateId> : IRepository<TAggregate, TAggregateId>
        where TAggregate : AggregateBase<TAggregateId>, IAggregate<TAggregateId>

    {
        private readonly IEventStore _eventStore;

        private readonly IProcessor<TAggregateId> _publisher;

        public EventSourcingRepository(IEventStore eventStore, IProcessor<TAggregateId> publisher)
        {
            _eventStore = eventStore;
            _publisher = publisher;
        }

        public async Task<TAggregate> GetByIdAsync(TAggregateId id)
        {
            try
            {
                var aggregate = CreateEmptyAggregate();

                foreach (var @event in await _eventStore.ReadEventsAsync(id))
                {
                    (aggregate as AggregateBase<TAggregate>).ApplyEvent(@event.DomainEvent as IDomainEvent<TAggregate>, @event.EventNumber);
                }

                return aggregate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
            try
            {
                foreach (var @event in aggregate.GetUncommittedEvents())
                {
                    await _eventStore.AppendEventAsync(@event);
                    
                    _publisher.ProcessAsync(@event, @event.GetType());
                }

                aggregate.ClearUncommittedEvents();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to access persistence layer", ex);
            }
        }

        private TAggregate CreateEmptyAggregate()
        {
            return (TAggregate)typeof(TAggregate)
                    .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                        null, new Type[0], new ParameterModifier[0])
                    .Invoke(new object[0]);
        }
    }
}
