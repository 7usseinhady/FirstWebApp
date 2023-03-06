using Microsoft.EntityFrameworkCore;
using WebApp.Core.Bases;
using WebApp.Core.Interfaces;

namespace WebApp.Infrastructure.Repositories
{
    public class BaseEntityRepository<TEntity, TKey> : GenericRepository<TEntity>, IBaseEntityRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public BaseEntityRepository(DbContext context) : base(context)
        {
        }

        public void Detach(TKey id)
        {
            var oEntity = DbSet().Local.FirstOrDefault(x => x.Id.Equals(id));
            if (oEntity is not null)
                SetContextState(oEntity, EntityState.Detached);
        }

        public void DetachRang(IEnumerable<TKey> lIds)
        {
            var lEntity = DbSet().Local.Where(x => lIds.Contains(x.Id)).ToList();
            lEntity.ForEach(x => SetContextState(x, EntityState.Detached));
        }
    }
}
