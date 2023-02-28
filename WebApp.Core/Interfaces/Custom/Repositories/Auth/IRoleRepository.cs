using WebApp.DataTransferObjects.Filters.Auth;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Core.Interfaces.Custom.Repositories.Auth
{
    public interface IRoleRepository : IGenericRepository<IdentityRole>
    {
        Task<IQueryable<IdentityRole>> BuildRoleQueryAsync(RoleFilter roleFilter);
    }
}
