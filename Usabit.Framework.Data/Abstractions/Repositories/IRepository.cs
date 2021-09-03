using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Usabit.Framework.Data.Abstractions.Repositories
{
    public interface IRepository
    {
        Task<ICollection<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes) where TEntity : class;
        Task<ICollection<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, params string[] includes) where TEntity : class;
        Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes) where TEntity : class;
        Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, params string[] includes) where TEntity : class;
        Task InsertAsync<TEntity>(TEntity entity) where TEntity : class;
        Task InsertAsync<TEntity>(ICollection<TEntity> entity, bool autoDetectChangesEnabled = true) where TEntity : class;
        Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
        Task UpdateAsync<TEntity>(ICollection<TEntity> entity, bool autoDetectChangesEnabled = true) where TEntity : class;
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
        Task DeleteAsync<TEntity>(ICollection<TEntity> entity, bool autoDetectChangesEnabled = true) where TEntity : class;
    }
}