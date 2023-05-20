using WebApp.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using WebApp.Core.Entities.Auth;
using System.Runtime.ExceptionServices;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;

namespace WebApp.Infrastructure.Repositories.Custom.Auth
{
    public class UserRepository : GenericRepository<User, UserFilterRequestDto>
    {
        private readonly WebAppDBContext _dbContext;

        public UserRepository(WebAppDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async override Task<IQueryable<User>> FilterQueryAsync(IQueryable<User> query, UserFilterRequestDto filterRequestDto)
        {
            try
            {
                // Where
                if (filterRequestDto is not null)
                {
                    if (!string.IsNullOrEmpty(filterRequestDto.RoleName))
                    {
                        var role = await _dbContext.Roles.Where(x => x.Name == filterRequestDto.RoleName).SingleOrDefaultAsync();
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

                    if (!string.IsNullOrEmpty(filterRequestDto.Id))
                        query = query.Where(x => x.Id == filterRequestDto.Id);

                    if (!string.IsNullOrEmpty(filterRequestDto.FullName))
                        query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(filterRequestDto.FullName));

                    if (!string.IsNullOrEmpty(filterRequestDto.Username))
                        query = query.Where(x => x.UserName!.Contains(filterRequestDto.Username));

                    if (!string.IsNullOrEmpty(filterRequestDto.Email))
                        query = query.Where(x => x.Email!.Contains(filterRequestDto.Email));

                    if (!string.IsNullOrEmpty(filterRequestDto.PhoneNumber))
                        query = query.Where(x => x.LocalPhoneNumber!.Contains(filterRequestDto.PhoneNumber));

                    if (!string.IsNullOrEmpty(filterRequestDto.SecondPhoneNumber))
                        query = query.Where(x => x.SecondLocalPhoneNumber!.Contains(filterRequestDto.SecondPhoneNumber));

                    if (!string.IsNullOrEmpty(filterRequestDto.CommonKeyWord))
                        query = query.Where(x => x.FirstName.Contains(filterRequestDto.CommonKeyWord) || x.LastName.Contains(filterRequestDto.CommonKeyWord));

                    if (filterRequestDto.IsFemale == 0 || filterRequestDto.IsFemale == 1)
                        query = query.Where(x => x.IsFemale == Convert.ToBoolean(filterRequestDto.IsFemale));

                    if (filterRequestDto.IsInactive == 0 || filterRequestDto.IsInactive == 1)
                        query = query.Where(x => x.IsInactive == Convert.ToBoolean(filterRequestDto.IsInactive));

                    if (filterRequestDto.CreationDateFrom.HasValue)
                        query = query.Where(x => x.UserInsertDate >= filterRequestDto.CreationDateFrom.Value.Date);

                    if (filterRequestDto.CreationDateTo.HasValue)
                        query = query.Where(x => x.UserInsertDate <= filterRequestDto.CreationDateTo.Value.Date);
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
