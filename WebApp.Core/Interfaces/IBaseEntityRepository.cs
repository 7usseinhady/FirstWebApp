using WebApp.Core.Bases;

namespace WebApp.Core.Interfaces
{
    public interface IBaseEntityRepository<TEntity, TKey> : IGenericRepository<TEntity>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        void Detach(TKey id);
        void DetachRang(IEnumerable<TKey> lIds);
    }
}
