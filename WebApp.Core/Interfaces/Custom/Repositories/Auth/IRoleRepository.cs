using Microsoft.AspNetCore.Identity;
using WebApp.Core.Entities.Auth;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;

namespace WebApp.Core.Interfaces.Custom.Repositories.Auth
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        IQueryable<Role> BuildRoleQuery(RoleFilterRequestDto roleFilter);
    }
}
