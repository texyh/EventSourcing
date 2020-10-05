using EventSourcing.Domain.Core;
using System.Threading.Tasks;

namespace EventSourcing.Services
{
    public interface IDomainEventHandler<TEvent> 
    {
        Task HandleAsync(TEvent @event);
    }
}
