using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProteinCase.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CurrencyDbContext _dbContext;

        public GenericRepository(CurrencyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression).AsQueryable();
        }

        public async Task<int> SaveChanges()
        {
            var effectedRowCount = await _dbContext.SaveChangesAsync();
            if (effectedRowCount < 1)
            {
                throw new Exception("İşlem Başarısız!");
            }

            return effectedRowCount;
        }
    }
}