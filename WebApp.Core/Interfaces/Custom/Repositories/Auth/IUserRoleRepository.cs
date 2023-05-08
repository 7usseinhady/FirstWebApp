using WebApp.Core.Entities.Auth;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;

namespace WebApp.Core.Interfaces.Custom.Repositories.Auth
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        IQueryable<UserRole> BuildUserRoleQuery(UserRoleFilterRequestDto userRoleFilter);
    }
}
