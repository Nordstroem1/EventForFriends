using Application.Interfaces;
using Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly mySqlDb _mySqlDb;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(mySqlDb mySqlDb)
        {
            _mySqlDb = mySqlDb;
            _dbSet = _mySqlDb.Set<T>();
        }
        public async Task<T>? GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _mySqlDb.SaveChangesAsync();
            return entity;
        }
        public async Task<T> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _mySqlDb.SaveChangesAsync();
            return entity;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _mySqlDb.SaveChangesAsync();
            return entity;
        }
    }
}
