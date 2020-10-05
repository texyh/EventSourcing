using System.Threading.Tasks;

namespace EventSourcing.Domain.DataAccess.ReadModel
{
    public interface IRepository<T> : IReadOnlyRepository<T>
        where T : IReadEntity
    {
        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

    }
}
