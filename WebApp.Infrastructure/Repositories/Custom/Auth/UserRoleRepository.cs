using WebApp.DataTransferObjects.Filters.Auth;
using WebApp.Infrastructure.DBContexts;
using Microsoft.AspNetCore.Identity;
using WebApp.Core.Interfaces.Custom.Repositories.Auth;
using WebApp.Core.Entities.Auth;

namespace WebApp.Infrastructure.Repositories.Custom.Auth
{
    public class UserRoleRepository : GenericRepository<UserRoles>, IUserRoleRepository
    {
        public UserRoleRepository(WebAppDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<IQueryable<UserRoles>> BuildUserRoleQuery(UserRoleFilter userRoleFilter)
        {
            try
            {
                var query = Query();

                // Where
                if (userRoleFilter is not null)
                {
                    if (!string.IsNullOrEmpty(userRoleFilter.UserId))
                        query = query.Where(x => x.UserId == userRoleFilter.UserId);

                    if (!string.IsNullOrEmpty(userRoleFilter.RoleId))
                        query = query.Where(x => x.RoleId == userRoleFilter.RoleId);
                }
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
