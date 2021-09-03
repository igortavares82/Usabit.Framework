using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Usabit.Framework.Data.Abstractions.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        Task InsertAsync(TEntity entity);
        Task InsertAsync(ICollection<TEntity> entity, bool autoDetectChangesEnabled = true);
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(ICollection<TEntity> entity, bool autoDetectChangesEnabled = true);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(ICollection<TEntity> entity, bool autoDetectChangesEnabled = true);
    }
}