using Leaguerly.Repositories.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace Leaguerly.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : class, IDbEntity<TKey>
    {
        IQueryable<TEntity> Query { get; }
        IEnumerable<TEntity> All();
        TEntity Get(TKey id);
        TKey Create(TEntity newEntity);
        void Update(TEntity updatedEntity);
        void Delete(TKey id);
    }
}