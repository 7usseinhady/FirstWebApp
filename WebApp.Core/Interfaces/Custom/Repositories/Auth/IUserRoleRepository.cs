using WebApp.DataTransferObjects.Filters.Auth;
using WebApp.Core.Entities.Auth;

namespace WebApp.Core.Interfaces.Custom.Repositories.Auth
{
    public interface IUserRoleRepository : IGenericRepository<UserRoles>
    {
        Task<IQueryable<UserRoles>> BuildUserRoleQuery(UserRoleFilter userRoleFilter);
    }
}
