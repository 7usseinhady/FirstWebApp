using WebApp.Core.Entities.Auth;

namespace WebApp.Core.Interfaces
{
    public interface IUserModification
    {
        string? ModifiedById { get; set; }
        User? ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
        
    }
}
