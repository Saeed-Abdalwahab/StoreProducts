

using Microsoft.EntityFrameworkCore;
using Repository.Abstraction;
using Store_TechniaclTask.DAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class Repository<Tentity> : IRepository<Tentity> where Tentity : class
    {
        private readonly DbContext _DbContext;
        private readonly DbSet<Tentity> _DbSet;
        public readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._DbContext = this._unitOfWork.context;
            this._DbSet = this._DbContext.Set<Tentity>();
        }

        public Tentity Add(Tentity entity)
        {
            if (entity == null) return null;
            this._DbSet.Add(entity);
            return entity;
        }

        public async Task<Tentity> AddAsync(Tentity entity)
        {
            if (entity == null) return null;
            await this._DbSet.AddAsync(entity);
            return entity;
        }
        public Tentity Update(Tentity entity)
        {
            this._DbSet.Attach(entity);
            this._DbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }


        public IEnumerable<Tentity> AddRange(IEnumerable<Tentity> entities)
        {
            if (entities == null) return null;
            this._DbSet.AddRange(entities);
            return entities;
        }
        public IEnumerable<Tentity> Update(IEnumerable<Tentity> entities)
        {
            if (entities == null) return null;
            this._DbSet.UpdateRange(entities);
            return entities;
        }

        public IQueryable<Tentity> Find(Expression<Func<Tentity, bool>> predicate)
        {
            return this._DbSet.Where(predicate);
        }

        public Tentity FristOrDefault(Expression<Func<Tentity, bool>> predicate)
        {
            return this._DbSet.FirstOrDefault(predicate);
        }


        public Tentity Get(params object[] keyvalue)
        {
            var entity = this._DbSet.Find(keyvalue);
            //this._DbSet.Attach(entity);
            //this._DbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public async Task<Tentity> GetAsync(params object[] keyvalue)
        {
            return await this._DbSet.FindAsync(keyvalue);
        }

        public IQueryable<Tentity> GetAll()
        {
            return this._DbSet;
        }
        public IQueryable<Tentity> GetAll(Expression<Func<Tentity, bool>> predicate)
        {
            return this._DbSet.Where(predicate);
        }
        public async Task<IEnumerable<Tentity>> GetAllAsync()
        {
            return await this._DbSet.ToListAsync();
        }
        public async Task<IEnumerable<Tentity>> GetAllAsync(Expression<Func<Tentity, bool>> predicate)
        {
            return await this._DbSet.Where(predicate).ToListAsync();
        }

        public bool Remove(Tentity entity)
        {
            this._DbSet.Remove(entity);
            return true;

        }
        public bool RemoveRange(IEnumerable<Tentity> entities)
        {
            this._DbSet.RemoveRange(entities);
            return true;
        }

        public bool Any(Expression<Func<Tentity, bool>> predicate)
        {
            return this._DbSet.Any(predicate);
        }
        public async Task<bool> AnyAsync(Expression<Func<Tentity, bool>> predicate)
        {
            return await this._DbSet.AnyAsync(predicate);
        }

        public void Detached(Tentity entity)
        {
            //this._DbSet.Attach(entity);
            this._DbContext.Entry(entity).State = EntityState.Detached;
        }
        public void Detached(IEnumerable<Tentity> entities)
        {
            //this._DbSet.Attach(entity);
            try
            {
                if (entities.Count() > 0)
                    this._DbContext.Entry(entities).State = EntityState.Detached;
            }
            catch
            {

            }
        }

        public IEnumerable<string> GetDependenciesNames(Tentity entity, params string[] Ignore)
        {
            var IEnumerableType = typeof(System.Collections.IEnumerable);
            Type StringType = typeof(string);
            if (entity == null)
            {
                return Enumerable.Empty<string>();
            }
            var dependents = new List<string>();
            var sdf = entity.GetType()
                    .GetProperties().ToList();
            var properties = entity.GetType()
                .GetProperties()
                .Where(p => IEnumerableType.IsAssignableFrom(p.PropertyType) && !StringType.IsAssignableFrom(p.PropertyType));

            foreach (var property in properties)
            {
                var values = property.GetValue(entity);
                //var children = (from object value in values select value).ToList();
                if (values != null)
                {
                    dependents.Add(property.Name);
                }
            }

            return dependents.Where(x => Ignore.Contains(x) == false);
        }
        public static IEnumerable<string> GetDependenciesNamesstatic(Tentity entity)
        {
            var IEnumerableType = typeof(System.Collections.IEnumerable);
            Type StringType = typeof(string);
            if (entity == null)
            {
                return Enumerable.Empty<string>();
            }
            var dependents = new List<string>();
            var sdf = entity.GetType()
                    .GetProperties().ToList();
            var properties = entity.GetType()
                .GetProperties()
                .Where(p => IEnumerableType.IsAssignableFrom(p.PropertyType) && !StringType.IsAssignableFrom(p.PropertyType));

            foreach (var property in properties)
            {
                var values = property.GetValue(entity);
                //var children = (from object value in values select value).ToList();
                if (values != null)
                {
                    dependents.Add(property.Name);
                }
            }
            return dependents;
        }
        public virtual async Task<IPagedList<Tentity>> GetAllPagedAsync(Func<IQueryable<Tentity>, IQueryable<Tentity>> func = null,
    int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = true)
        {
            var query = AddDeletedFilter(_DbSet, includeDeleted);

            query = func != null ? func(query) : query;

            return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
        }
        protected virtual IQueryable<Tentity> AddDeletedFilter(IQueryable<Tentity> query, in bool includeDeleted)
        {
            if (includeDeleted)
                return query;

            if (typeof(Tentity).GetInterface(nameof(ISoftDeletedEntity)) == null)
                return query;

            return query.OfType<ISoftDeletedEntity>().Where(entry => !entry.IsDeleted).OfType<Tentity>();
        }

    }
}
