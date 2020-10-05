using System.Threading.Tasks;
using System;
using EventSourcing.Domain.Core;

namespace EventSourcing.Domain.DataAccess
{
    public interface IRepository<TAggregate, TAggregateId>
        where TAggregate : IAggregate<TAggregateId>
    {
        Task<TAggregate> GetByIdAsync(TAggregateId id);

        Task SaveAsync(TAggregate aggregate);
    }
}