using Store_TechniaclTask.DAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstraction
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(params object[] keyvalue);
        void Detached(TEntity entity);
        void Detached(IEnumerable<TEntity> entities);
        Task<TEntity> GetAsync(params object[] keyvalue);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FristOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        IEnumerable<TEntity> Update(IEnumerable<TEntity> entities);
        bool Remove(TEntity entity);
        bool RemoveRange(IEnumerable<TEntity> entities);
        IEnumerable<string> GetDependenciesNames(TEntity entity, params string[] Ignore);
        Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
         int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = true);

    }
}
