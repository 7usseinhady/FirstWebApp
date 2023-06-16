using Microsoft.EntityFrameworkCore.Storage;
using WebApp.Core.Entities.Auth;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;

namespace WebApp.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        #region Auth
        public IGenericRepository<Role, RoleFilterRequestDto> Roles { get; }
        public IGenericRepository<User, UserFilterRequestDto> Users { get; }
        public IGenericRepository<UserRole, UserRoleFilterRequestDto> UserRoles { get; }
        #endregion

        IDbContextTransaction Transaction();
        int Complete();
        Task<int> CompleteAsync();
    }
}
