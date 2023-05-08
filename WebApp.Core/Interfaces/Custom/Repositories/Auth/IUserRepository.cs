using WebApp.Core.Entities.Auth;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;

namespace WebApp.Core.Interfaces.Custom.Repositories.Auth
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IQueryable<User>> BuildUserQueryAsync(UserFilterRequestDto userFilter);
    }
}
