using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourcing.Domain.DataAccess.EventStore
{
    public class EventEntity
    {
        public string AggregateId { get; set; }

        public Guid EventId { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public long AggregateVersion {get; set;}
    }
}
