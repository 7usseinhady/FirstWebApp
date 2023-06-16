using WebApp.Core.Interfaces;
using WebApp.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore.Storage;
using WebApp.Infrastructure.Repositories.Custom.Auth;
using WebApp.Core.Entities.Auth;
using WebApp.SharedKernel.Dtos.Auth.Request.Filters;

namespace WebApp.Infrastructure.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly WebAppDBContext _context;

        #region Auth
        public IGenericRepository<Role, RoleFilterRequestDto> Roles { get; private set; }
        public IGenericRepository<User, UserFilterRequestDto> Users { get; private set; }
        public IGenericRepository<UserRole, UserRoleFilterRequestDto> UserRoles { get; private set; }
        #endregion


        public UnitOfWork(WebAppDBContext context)
        {
            _context = context;

            #region Auth
            Roles = new RoleRepository(_context);
            Users = new UserRepository(_context);
            UserRoles = new UserRoleRepository(_context);
            #endregion
        }

        public IDbContextTransaction Transaction()
        {
            return _context.Database.BeginTransaction();
        }
        
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
