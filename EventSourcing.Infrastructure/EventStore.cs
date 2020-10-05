using EventSourcing.Domain.Core;
using EventSourcing.Domain.DataAccess.EventStore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Infrastructure
{
    public class EventStore : IEventStore
    {
        private readonly DataContext _dbContext;

        public EventStore(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AppendResult> AppendEventAsync<TAggregateId>(IDomainEvent<TAggregateId> @event)
        {
            try
            {
                var entity = new EventEntity
                {
                    AggregateId = @event.AggregateId.ToString(),
                    Data = Serialize(@event),
                    EventId = @event.EventId,
                    Type = @event.GetType().AssemblyQualifiedName,
                    AggregateVersion = @event.AggregateVersion
                };

                _dbContext.Events.Add(entity);
                await _dbContext.SaveChangesAsync();

                return new AppendResult(@event.AggregateVersion  + 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Event<TAggregateId>>> ReadEventsAsync<TAggregateId>(TAggregateId id)
        {
            try
            {
                var events = await _dbContext.Events.AsNoTracking().Where(x => x.AggregateId == id.ToString()).ToListAsync();
                return events.Select(x => new Event<TAggregateId>(
                    Deserialize<TAggregateId>(x.Type, x.Data),
                    x.AggregateVersion));
                
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string eventType, string data)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new PrivateSetterContractResolver() };
            return (IDomainEvent<TAggregateId>)JsonConvert.DeserializeObject(data, Type.GetType(eventType), settings);
        }

        private string Serialize<TAggregateId>(IDomainEvent<TAggregateId> @event)
        {
            return JsonConvert.SerializeObject(@event);
        }
    }
}
