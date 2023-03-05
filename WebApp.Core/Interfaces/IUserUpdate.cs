using WebApp.Core.Entities.Auth;

namespace WebApp.Core.Interfaces
{
    public interface IUserUpdate
    {
        string? UserUpdateId { get; set; }
        User? UserUpdate { get; set; }
        DateTime? UserUpdateDate { get; set; }
        
    }
}
