using Microsoft.EntityFrameworkCore;
using WebApp.Core.Bases;
using WebApp.Core.Interfaces;
using WebApp.SharedKernel.Bases;

namespace WebApp.Infrastructure.Repositories
{
    public abstract class BaseEntityRepository<TEntity, TKey, TFilter> : GenericRepository<TEntity, TFilter>, IBaseEntityRepository<TEntity, TKey, TFilter>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
        where TFilter : BaseFilterRequestDto
    {
        protected BaseEntityRepository(DbContext context) : base(context)
        {
        }

        #region Helpers: Other Entity State Methods
        public void Detach(TKey id)
        {
            var oEntity = DbSet().Local.FirstOrDefault(x => x.Id.Equals(id));
            if (oEntity is not null)
                SetEntityState(oEntity, EntityState.Detached);
        }

        public void DetachRang(IEnumerable<TKey> lIds)
        {
            var lEntity = DbSet().Local.Where(x => lIds.Contains(x.Id)).ToList();
            lEntity.ForEach(x => SetEntityState(x, EntityState.Detached));
        }
        #endregion
    }
}
