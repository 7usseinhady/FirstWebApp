using WebApp.DataTransferObjects.Filters.Auth;
using Microsoft.AspNetCore.Identity;
using WebApp.Infrastructure.DBContexts;
using WebApp.Core.Interfaces.Custom.Repositories.Auth;

namespace WebApp.Infrastructure.Repositories.Custom.Auth
{
    public class RoleRepository : GenericRepository<IdentityRole>, IRoleRepository
    {
        public RoleRepository(WebAppDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<IQueryable<IdentityRole>> BuildRoleQueryAsync(RoleFilter roleFilter)
        {
            try
            {
                var query = Query();

                // Where
                if (roleFilter is not null)
                {
                    if (!string.IsNullOrEmpty(roleFilter.Id))
                        query = query.Where(x => x.Id == roleFilter.Id);

                    if (!string.IsNullOrEmpty(roleFilter.Name))
                        query = query.Where(x => x.Name!.Contains(roleFilter.Name));
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
