using WebApp.Core.Entities.Auth;

namespace WebApp.Core.Interfaces
{
    public interface IUserInsertion
    {
        string? InsertedById { get; set; }
        User? InsertedBy { get; set; }
        DateTime? InsertedOn { get; set; }
    }
}
