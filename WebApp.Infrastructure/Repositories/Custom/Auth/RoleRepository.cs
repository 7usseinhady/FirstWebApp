using WebApp.DataTransferObjects.Filters.Auth;
using Microsoft.AspNetCore.Identity;
using WebApp.Infrastructure.DBContexts;
using WebApp.Core.Interfaces.Custom.Repositories.Auth;
using WebApp.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Infrastructure.Repositories.Custom.Auth
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly WebAppDBContext _dbContext;
        public RoleRepository(WebAppDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<Role>> BuildRoleQueryAsync(RoleFilter roleFilter)
        {
            try
            {
                var query = Query();

                var userRoleQuery = from role in query
                                    join userRole in _dbContext.UserRoles
                                    on role.Id equals userRole.RoleId
                                    select new { role, userRole };

                // Where
                if (roleFilter is not null)
                {
                    if (!string.IsNullOrEmpty(roleFilter.Id))
                        userRoleQuery = userRoleQuery.Where(x => x.role.Id == roleFilter.Id);

                    if (!string.IsNullOrEmpty(roleFilter.Name))
                        userRoleQuery = userRoleQuery.Where(x => x.role.Name!.Contains(roleFilter.Name));
                }

                //query = userRoleQuery.Select(x => new Role()
                //{
                //    Id = x.role.Id,
                //    Name = x.role.Name,
                //    UserCount= x.,

                //});

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
