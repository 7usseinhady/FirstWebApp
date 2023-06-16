using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApp.Core.Interfaces;
using WebApp.SharedKernel.Bases;

namespace WebApp.Infrastructure.Repositories
{
    public abstract class GenericRepository<TEntity, TFilter> : IGenericRepository<TEntity, TFilter>
        where TEntity : class
        where TFilter : BaseFilterRequestDto
    {
        private readonly DbContext _context;
        protected GenericRepository(DbContext context)
        {
            _context = context;
        }

        #region Commands Methods
        // Sync
        public TEntity Add(TEntity entity)
        {
            DbSet().Add(entity);
            return entity;
        }
        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            DbSet().AddRange(entities);
            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            _context.Update(entity);
            return entity;
        }
        public IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.UpdateRange(entities);
            return entities;
        }

        public void DeleteById(object id)
        {
            var element = DbSet().Find(id);
            Delete(element!);
        }

        public void DeleteRangeByIds(IEnumerable<object> ids)
        {
            List<TEntity> lTEntity = new List<TEntity>();
            foreach (int id in ids)
                lTEntity.Add(DbSet()?.Find(id)!);
            DeleteRange(lTEntity);
        }
        public void Delete(TEntity entity)
        {
            DbSet().Remove(entity);
        }
        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbSet().RemoveRange(entities);
        }

        // ASync
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbSet().AddAsync(entity);
            return entity;
        }
        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await DbSet().AddRangeAsync(entities);
            return entities;
        }
        #endregion

        #region Query Methods
        public virtual Task<IQueryable<TEntity>> BuildBaseQueryAsync()
        {
            return Task.FromResult(DbSet().AsQueryable().AsNoTracking());
        }
        
        public abstract Task<IQueryable<TEntity>> FilterQueryAsync(IQueryable<TEntity> query, TFilter filterRequestDto);
        #endregion

        #region Helpers Methods
        private IEntityType? GetEntityType()
        {
            return _context?.Model?.FindEntityType(typeof(TEntity));
        }
        protected DbSet<TEntity> DbSet()
        {
            return _context.Set<TEntity>();
        }

        #region MetaData Methods
        public string? GetSchema()
        {
            return GetEntityType()?.GetSchema();
        }
        public string? GetTableName()
        {
            return GetEntityType()?.GetTableName();
        }
        public string? GetPrimaryKeyColmunName()
        {
            return GetEntityType()?.GetKeys().Select(p => p.GetName()).FirstOrDefault();
        }
        #endregion

        #region Entity State Methods
        public void Attach(TEntity entity)
        {
            DbSet().Attach(entity);
        }
        public void AttachRange(IEnumerable<TEntity> entities)
        {
            DbSet().AttachRange(entities);
        }
        public void SetEntityState(TEntity entity, EntityState state)
        {
            _context.Entry(entity).State = state;
        }
        #endregion
        #endregion


    }
}
