using WebApp.Core.Entities.Auth;

namespace WebApp.Core.Interfaces
{
    public interface IUserInsert
    {
        string? UserInsertId { get; set; }
        User? UserInsert { get; set; }
        DateTime? UserInsertDate { get; set; }
    }
}
