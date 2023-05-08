using WebApp.Infrastructure.DBContexts;
using WebApp.Core.Interfaces.Custom.Repositories.Auth;
using WebApp.Core.Entities.Auth;
using System.Runtime.ExceptionServices;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;

namespace WebApp.Infrastructure.Repositories.Custom.Auth
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(WebAppDBContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<UserRole> BuildUserRoleQuery(UserRoleFilterRequestDto userRoleFilter)
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
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                return null!;
            }
        }
    }
}
