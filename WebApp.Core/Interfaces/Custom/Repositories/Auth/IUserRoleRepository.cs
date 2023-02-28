using WebApp.DataTransferObjects.Filters.Auth;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Core.Interfaces.Custom.Repositories.Auth
{
    public interface IUserRoleRepository : IGenericRepository<IdentityUserRole<string>>
    {
        Task<IQueryable<IdentityUserRole<string>>> BuildUserRoleQuery(UserRoleFilter userRoleFilter);
    }
}
