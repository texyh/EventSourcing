using EventSourcing.Domain.DataAccess.ReadModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Infrastructure
{
    public class ReadRepository<T> : IRepository<T> where T : class, IReadEntity
    {
        private readonly DataContext _dbContext;

        public ReadRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task InsertAsync(T entity)
        {
             _dbContext.Set<T>().Add(entity);
            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return _dbContext.SaveChangesAsync();
        }
    }
}
