
namespace WebApp.Core.Bases
{
    public class BaseTreeEntity<TKey> : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public virtual TKey ParentId { get; set; } = default!;
    }
}
