using WebApp.DataTransferObjects.Filters.Auth;
using WebApp.Core.Entities.Auth;

namespace WebApp.Core.Interfaces.Custom.Repositories.Auth
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IQueryable<User>> BuildUserQueryAsync(UserFilter userFilter);
    }
}
