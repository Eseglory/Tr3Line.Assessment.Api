using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tr3Line.Assessment.Api.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tr3Line.Assessment.Helpers;

namespace Tr3Line.Assessment.Api.Repo
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        protected readonly DataContext _context;
        public DataRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddRangeAsync(IEnumerable<T> entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await _context.Set<T>().AddRangeAsync(entity);
        }

        public void UpdateRangeAsync(IEnumerable<T> entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

             _context.Set<T>().UpdateRange(entity);
        }


        public async Task<bool> CreateAsync(T entity)
        {
            if (entity == null) return false;

            await _context.Set<T>().AddAsync(entity);

            return true;
        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _context.Set<T>().Remove(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null) return null;

            return entity;
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public EntityEntry<TEntity> Update<TEntity>([NotNull] TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}
