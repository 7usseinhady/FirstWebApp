using Microsoft.EntityFrameworkCore.Storage;
using WebApp.Core.Interfaces.Custom.Repositories.Auth;

namespace WebApp.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IRoleRepository roles { get; }
        public IUserRepository users { get; }
        public IUserRoleRepository userRoles { get; }

        IDbContextTransaction Transaction();

        int Complete();
        Task<int> CompleteAsync();
    }
}
