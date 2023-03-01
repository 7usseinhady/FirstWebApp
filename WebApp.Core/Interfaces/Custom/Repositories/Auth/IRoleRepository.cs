using WebApp.DataTransferObjects.Filters.Auth;
using Microsoft.AspNetCore.Identity;
using WebApp.Core.Entities.Auth;

namespace WebApp.Core.Interfaces.Custom.Repositories.Auth
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<IQueryable<Role>> BuildRoleQueryAsync(RoleFilter roleFilter);
    }
}
