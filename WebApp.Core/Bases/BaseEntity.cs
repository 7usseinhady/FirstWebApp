using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.Bases
{
    public class BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }
    }
}
