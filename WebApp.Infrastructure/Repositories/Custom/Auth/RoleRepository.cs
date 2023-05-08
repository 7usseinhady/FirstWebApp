using WebApp.DataTransferObject.Filters.Auth;
using WebApp.Infrastructure.DBContexts;
using WebApp.Core.Interfaces.Custom.Repositories.Auth;
using WebApp.Core.Entities.Auth;
using System.Runtime.ExceptionServices;

namespace WebApp.Infrastructure.Repositories.Custom.Auth
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly WebAppDBContext _dbContext;
        public RoleRepository(WebAppDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Role> BuildRoleQuery(RoleFilter roleFilter)
        {
            try
            {
                var query = Query();

                var userRoleGroupQuery = from userRole in _dbContext.UserRoles
                                         group userRole by userRole.RoleId into userRoleGroup
                                         select new {
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
                            Name= role.Name,
                            ConcurrencyStamp= role.ConcurrencyStamp,
                            NormalizedName= role.NormalizedName,

                            UserCount = userRoleCount.userCount,

                            UserInsertId = role.UserInsertId,
                            UserInsertDate= role.UserInsertDate,
                            UserInsert = role.UserInsert,
                            UserUpdateId= role.UserUpdateId,
                            UserUpdateDate= role.UserUpdateDate,
                            UserUpdate = role.UserUpdate
                        };

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
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                return null!;
            }
        }
    }
}
