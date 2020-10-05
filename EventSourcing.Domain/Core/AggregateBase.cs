using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSourcing.Domain.Core
{
    public abstract class AggregateBase<TId> : IAggregate<TId>
    {
        public const long NewAggregateVersion = -1;

        private readonly ICollection<IDomainEvent<TId>> _uncommittedEvents = new LinkedList<IDomainEvent<TId>>();

        protected Dictionary<Type, Action<IDomainEvent<TId>>> eventAppliers;

        private long _version = NewAggregateVersion;

        public TId Id { get; protected set; }

        long Version => _version;

        public AggregateBase()
        {
            this.eventAppliers = new Dictionary<Type, Action<IDomainEvent<TId>>>();
            this.RegisterAppliers();
        }

        protected abstract void RegisterAppliers();

        protected void RegisterApplier<TEvent>(Action<TEvent> applier) where TEvent : IDomainEvent<TId>
        {
            this.eventAppliers.Add(typeof(TEvent), (x) => applier((TEvent)x));
        }

        public void ApplyEvent(IDomainEvent<TId> @event, long version)
        {
            if (!_uncommittedEvents.Any(x => Equals(x.EventId, @event.EventId)))
            {
                var evtType = @event.GetType();
                this.eventAppliers[evtType](@event);
                _version = version;
            }
        }


        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public IEnumerable<IDomainEvent<TId>> GetUncommittedEvents()
        {
            return _uncommittedEvents.AsEnumerable();
        }

        protected void RaiseEvent<TEvent>(TEvent @event)
            where TEvent : DomainEventBase<TId>
        {
            IDomainEvent<TId> eventWithAggregate = @event.CreateWithAggregate(
                Equals(Id, default(TId)) ? @event.AggregateId : Id,
                _version);

            ApplyEvent(eventWithAggregate, _version + 1);
            _uncommittedEvents.Add(eventWithAggregate);
        }
    }
}
