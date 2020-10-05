using EventSourcing.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Domain.DataAccess.EventStore
{
    public interface IEventStore
    {
        Task<IEnumerable<Event<TAggregateId>>> ReadEventsAsync<TAggregateId>(TAggregateId id);
        Task<AppendResult> AppendEventAsync<TAggregateId>(IDomainEvent<TAggregateId> @event);
    }
}
