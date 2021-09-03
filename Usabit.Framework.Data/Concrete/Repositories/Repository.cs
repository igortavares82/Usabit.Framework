using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Usabit.Framework.Data.Abstractions.Repositories;

namespace Usabit.Framework.Data.Concrete.Repositories
{
    public class Repository : IRepository, IDisposable
    {
        private bool _disposed = false;
        protected IHttpContextAccessor _httpContextAccessor;
        protected DbContext _context;

        public Repository() { }

        public Repository(DbContext context) => _context = context;

        public Repository(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public Repository(DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Remove(entity);
        }

        public async Task DeleteAsync<TEntity>(ICollection<TEntity> entity, bool autoDetectChangesEnabled = true) where TEntity : class
        {
            await CollectionHandlerAsync(entity, it => _context.Entry(it).State = EntityState.Deleted, autoDetectChangesEnabled);
        }

        public async Task<ICollection<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
            includes?.ToList().ForEach(it => query = query.Include(it));

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<ICollection<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, params string[] includes) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
            includes?.ToList().ForEach(it => query = query.Include(it));

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
            includes?.ToList().ForEach(it => query = query.Include(it));

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, params string[] includes) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
            includes?.ToList().ForEach(it => query = query.Include(it));

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _context.AddAsync(entity);
        }

        public async Task InsertAsync<TEntity>(ICollection<TEntity> entity, bool autoDetectChangesEnabled = true) where TEntity : class
        {
            await CollectionHandlerAsync(entity, it => _context.Add(it), autoDetectChangesEnabled);
        }

        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Update(entity);
        }

        public async Task UpdateAsync<TEntity>(ICollection<TEntity> entity, bool autoDetectChangesEnabled = true) where TEntity : class
        {
            await CollectionHandlerAsync(entity, it => _context.Entry(it).State = EntityState.Modified, autoDetectChangesEnabled);
        }

        protected Guid GetIdentityId()
        {
            string type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid";
            return Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(type).Value);
        }

        private async Task CollectionHandlerAsync<TEntity>(ICollection<TEntity> entity, Action<TEntity> handler, bool autoDetectChangesEnabled = true)
        {
            try
            {
                AutoDetectChanges(autoDetectChangesEnabled);
                entity.ToList().ForEach(it => handler(it));
            }
            finally
            {
                AutoDetectChanges(true);
            }
        }

        protected void AutoDetectChanges(bool autoDetectChanges) => _context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _context.Dispose();
                _httpContextAccessor = null;
            }

            _disposed = true;
            GC.Collect();
        }
    }
}
