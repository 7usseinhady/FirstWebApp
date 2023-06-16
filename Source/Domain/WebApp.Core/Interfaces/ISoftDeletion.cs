using WebApp.Core.Entities.Auth;

namespace WebApp.Core.Interfaces
{
    public interface ISoftDeletion
    {
        public bool IsSoftDeleted { get; set; }

        string? DeletedById { get; set; }
        User? DeletedBy { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
