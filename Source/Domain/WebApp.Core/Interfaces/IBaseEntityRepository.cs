using WebApp.Core.Bases;
using WebApp.SharedKernel.Bases;

namespace WebApp.Core.Interfaces
{
    public interface IBaseEntityRepository<TEntity, TKey, TFilter> : IGenericRepository<TEntity, TFilter>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
        where TFilter : BaseFilterRequestDto
    {
        #region Helpers: Other Entity State Methods
        void Detach(TKey id);
        void DetachRang(IEnumerable<TKey> lIds);
        #endregion
    }
}
