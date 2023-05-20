using WebApp.Infrastructure.DBContexts;
using WebApp.Core.Entities.Auth;
using System.Runtime.ExceptionServices;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;

namespace WebApp.Infrastructure.Repositories.Custom.Auth
{
    public class UserRoleRepository : GenericRepository<UserRole, UserRoleFilterRequestDto>
    {
        public UserRoleRepository(WebAppDBContext dbContext) : base(dbContext)
        {
        }

        public override Task<IQueryable<UserRole>> FilterQueryAsync(IQueryable<UserRole> query, UserRoleFilterRequestDto filterRequestDto)
        {
            try
            {
                // Where
                if (filterRequestDto is not null)
                {
                    if (!string.IsNullOrEmpty(filterRequestDto.UserId))
                        query = query.Where(x => x.UserId == filterRequestDto.UserId);

                    if (!string.IsNullOrEmpty(filterRequestDto.RoleId))
                        query = query.Where(x => x.RoleId == filterRequestDto.RoleId);
                }
                return Task.FromResult(query);
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                return null!;
            }
        }
    }
}
