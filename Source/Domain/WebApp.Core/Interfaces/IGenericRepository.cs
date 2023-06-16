using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Bases;

namespace WebApp.Core.Interfaces
{
    public interface IGenericRepository<TEntity, TFilter>
        where TEntity : class
        where TFilter : BaseFilterRequestDto
    {
        #region Commands Methods
        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);
        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);

        void DeleteById(object id);
        void DeleteRangeByIds(IEnumerable<object> ids);

        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);

        //ASync
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
        #endregion

        #region Query Methods
        Task<IQueryable<TEntity>> BuildBaseQueryAsync();
        Task<IQueryable<TEntity>> FilterQueryAsync(IQueryable<TEntity> query, TFilter filterRequestDto);
        #endregion

        #region Helpers Methods
        #region MetaData Methods
        string? GetSchema();
        string? GetTableName();
        string? GetPrimaryKeyColmunName();
        #endregion

        #region Entity State Methods
        void Attach(TEntity entity);
        void AttachRange(IEnumerable<TEntity> entities);
        void SetEntityState(TEntity entity, EntityState state);
        #endregion
        #endregion
    }
}
