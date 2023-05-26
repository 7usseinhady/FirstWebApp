using WebApp.Infrastructure.DBContexts;
using WebApp.Core.Entities.Auth;
using System.Runtime.ExceptionServices;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Infrastructure.Repositories.Custom.Auth
{
    public class RoleRepository : GenericRepository<Role, RoleFilterRequestDto>
    {
        private readonly WebAppDBContext _dbContext;
        public RoleRepository(WebAppDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async override Task<IQueryable<Role>> BuildBaseQueryAsync()
        {
            try
            {
                var query = await base.BuildBaseQueryAsync();

                var userRoleGroupQuery = from userRole in _dbContext.UserRoles.AsNoTracking()
                                         group userRole by userRole.RoleId into userRoleGroup
                                         select new
                                         {
                                             roleId = userRoleGroup.Key,
                                             userCount = userRoleGroup.Select(x => x.UserId).Count()
                                         };

                query = from role in query
                        join userRoleCount in userRoleGroupQuery
                        on role.Id equals userRoleCount.roleId into roleGrouping
                        from userRoleCount in roleGrouping.DefaultIfEmpty()
                        select new Role()
                        {
                            Id = role.Id,
                            Name = role.Name,
                            ConcurrencyStamp = role.ConcurrencyStamp,
                            NormalizedName = role.NormalizedName,
                            // --
                            UserCount = userRoleCount.userCount,
                            // --
                            InsertedById = role.InsertedById,
                            InsertedOn = role.InsertedOn,
                            InsertedBy = role.InsertedBy,
                            ModifiedById = role.ModifiedById,
                            ModifiedOn = role.ModifiedOn,
                            ModifiedBy = role.ModifiedBy
                        };

                return query;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                return null!;
            }
        }

        public override Task<IQueryable<Role>> FilterQueryAsync(IQueryable<Role> query, RoleFilterRequestDto filter)
        {
            try
            {
                if (filter is not null)
                {
                    if (!string.IsNullOrEmpty(filter.Id))
                        query = query.Where(x => x.Id == filter.Id);

                    if (!string.IsNullOrEmpty(filter.Name))
                        query = query.Where(x => x.Name!.Contains(filter.Name));
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
