using EventSourcing.Domain.Core;
using System.Threading.Tasks;

namespace EventSourcing.Services
{
    public interface IDomainEventHandler<TEvent> : IHandler
    {
        Task HandleAsync(TEvent @event);
    }
}
