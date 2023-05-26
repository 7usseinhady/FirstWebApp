using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApp.Core.Entities.Auth;
using WebApp.Core.Consts;
using Microsoft.AspNetCore.Http;
using WebApp.SharedKernel.Extensions;
using WebApp.Core.Interfaces;

namespace WebApp.Infrastructure.DBContexts
{
    public class WebAppDBContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly IHttpContextAccessor _accessor;
        public WebAppDBContext(DbContextOptions<WebAppDBContext> options, IHttpContextAccessor accessor) : base(options)
        {
            _accessor = accessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Identity
            builder.Entity<User>()
                .ToTable("Users", SqlSchemas.auth);
            builder.Entity<Role>()
                .ToTable("Roles", SqlSchemas.auth);
            builder.Entity<UserRole>()
                .ToTable("UserRoles", SqlSchemas.auth);
            builder.Entity<UserClaim>()
                .ToTable("UserClaims", SqlSchemas.auth);
            builder.Entity<UserLogin>()
                .ToTable("UserLogins", SqlSchemas.auth);
            builder.Entity<UserToken>()
                .ToTable("UserTokens", SqlSchemas.auth);
            builder.Entity<RoleClaim>()
                .ToTable("RoleClaims", SqlSchemas.auth);
            #endregion

            #region SeedIdentityRoles

            builder.Entity<Role>()
                .HasData(
            new Role()
            {
                Id = "d0b1b383-e64c-4f42-b732-7ffbe8f3666b",
                Name = SharedKernel.Consts.MainRoles.Admin,
                NormalizedName = SharedKernel.Consts.MainRoles.Admin.ToLower(),
                ConcurrencyStamp = "f4912daa-a439-43ea-9c5d-dbd590789948"
            }
            );
            #endregion

            #region SeedIdentityUser

            string ROLE_ID = "d0b1b383-e64c-4f42-b732-7ffbe8f3666b";
            string ADMIN_ID = "618fdfd2-f08b-413d-876a-04fec17f9e3f";

            //create user
            var appUser = new User
            {
                Id = ADMIN_ID,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin",
                NormalizedUserName = "admin".ToLower(),
                IsInactive = false,
                IsFemale = false,
                ConcurrencyStamp = ADMIN_ID
            };

            //set user password
            PasswordHasher<User> passwordHash = new PasswordHasher<User>();
            appUser.PasswordHash = passwordHash.HashPassword(appUser, "Admin@1234");

            //seed user
            builder.Entity<User>()
                .HasData(appUser);

            //set user role to admin
            builder.Entity<UserRole>()
                .HasData(new UserRole
                {
                    RoleId = ROLE_ID,
                    UserId = ADMIN_ID
                });
            #endregion

            #region Disable Cascading Delete
            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            #endregion

            #region Unique Index

            #endregion

        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            PreSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            PreSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void PreSaveChanges()
        {
            string userId = _accessor?.HttpContext?.User?.GetUserId()!;
            DateTime dateUtcNow = DateTime.UtcNow;
            
            var lEntityEntries = ChangeTracker.Entries().Where(e =>
                (e.Entity is IUserInsertion || e.Entity is IUserModification) &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in lEntityEntries)
            {
                if (entityEntry.State == EntityState.Added && entityEntry.Entity is IUserInsertion)
                {
                    ((IUserInsertion)entityEntry.Entity).InsertedById = userId;
                    ((IUserInsertion)entityEntry.Entity).InsertedOn = dateUtcNow;
                }
                else if (entityEntry.State == EntityState.Modified && entityEntry.Entity is IUserModification)
                {
                    ((IUserModification)entityEntry.Entity).ModifiedById = userId;
                    ((IUserModification)entityEntry.Entity).ModifiedOn = dateUtcNow;
                }
            }
        }
    }

}
