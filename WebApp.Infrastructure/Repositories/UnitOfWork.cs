using WebApp.Core.Interfaces;
using WebApp.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore.Storage;
using WebApp.Core.Interfaces.Custom.Repositories.Auth;
using WebApp.Infrastructure.Repositories.Custom.Auth;

namespace WebApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebAppDBContext _context;

        public IRoleRepository roles { get; private set; }
        public IUserRepository users { get; private set; }
        public IUserRoleRepository userRoles { get; private set; }



        public UnitOfWork(WebAppDBContext context)
        {
            _context = context;

            roles = new RoleRepository(_context);
            users = new UserRepository(_context);
            userRoles = new UserRoleRepository(_context);
        }
        public IDbContextTransaction Transaction()
        {
            return _context.Database.BeginTransaction();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
