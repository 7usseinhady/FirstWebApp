using WebApp.DataTransferObject.Filters.Auth;
using WebApp.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using WebApp.Core.Interfaces.Custom.Repositories.Auth;
using WebApp.Core.Entities.Auth;
using System.Runtime.ExceptionServices;

namespace WebApp.Infrastructure.Repositories.Custom.Auth
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly WebAppDBContext _dbContext;

        public UserRepository(WebAppDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<User>> BuildUserQueryAsync(UserFilter userFilter)
        {
            try
            {
                var query = Query();

                // Where
                if (userFilter is not null)
                {
                    if (!string.IsNullOrEmpty(userFilter.RoleName))
                    {
                        var role = await _dbContext.Roles.Where(x => x.Name == userFilter.RoleName).SingleOrDefaultAsync();
                        if (role is not null)
                        {
                            query = from user in DbSet()
                                    join userRole in _dbContext.UserRoles
                                    on user.Id equals userRole.UserId
                                    where userRole.RoleId == role.Id
                                    select user;

                            query = query.Distinct();
                        }
                        else
                            return null!;
                    }

                    if (!string.IsNullOrEmpty(userFilter.Id))
                        query = query.Where(x => x.Id == userFilter.Id);

                    if (!string.IsNullOrEmpty(userFilter.FullName))
                        query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(userFilter.FullName));

                    if (!string.IsNullOrEmpty(userFilter.Username))
                        query = query.Where(x => x.UserName!.Contains(userFilter.Username));

                    if (!string.IsNullOrEmpty(userFilter.Email))
                        query = query.Where(x => x.Email!.Contains(userFilter.Email));

                    if (!string.IsNullOrEmpty(userFilter.PhoneNumber))
                        query = query.Where(x => x.LocalPhoneNumber!.Contains(userFilter.PhoneNumber));

                    if (!string.IsNullOrEmpty(userFilter.SecondPhoneNumber))
                        query = query.Where(x => x.SecondLocalPhoneNumber!.Contains(userFilter.SecondPhoneNumber));

                    if (!string.IsNullOrEmpty(userFilter.CommonKeyWord))
                        query = query.Where(x => x.FirstName.Contains(userFilter.CommonKeyWord) || x.LastName.Contains(userFilter.CommonKeyWord));

                    if (userFilter.IsFemale == 0 || userFilter.IsFemale == 1)
                        query = query.Where(x => x.IsFemale == Convert.ToBoolean(userFilter.IsFemale));

                    if (userFilter.IsInactive == 0 || userFilter.IsInactive == 1)
                        query = query.Where(x => x.IsInactive == Convert.ToBoolean(userFilter.IsInactive));

                    if (userFilter.CreationDateFrom.HasValue)
                        query = query.Where(x => x.UserInsertDate >= userFilter.CreationDateFrom.Value.Date);

                    if (userFilter.CreationDateTo.HasValue)
                        query = query.Where(x => x.UserInsertDate <= userFilter.CreationDateTo.Value.Date);

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
