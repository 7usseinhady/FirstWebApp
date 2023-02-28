using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApp.Core.Entities.Auth;
using WebApp.SharedKernel.Consts;
using WebApp.Core.Consts;

namespace WebApp.Infrastructure.DBContexts
{
    public class WebAppDBContext : IdentityDbContext<User>
    {
        public WebAppDBContext(DbContextOptions<WebAppDBContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Identity
            builder.Entity<User>()
                .ToTable("Users", SqlSchemas.auth);
            builder.Entity<IdentityUserRole<string>>()
                .ToTable("UserRoles", SqlSchemas.auth);
            builder.Entity<IdentityUserClaim<string>>()
                .ToTable("UserClaims", SqlSchemas.auth);
            builder.Entity<IdentityUserLogin<string>>()
                .ToTable("UserLogins", SqlSchemas.auth);
            builder.Entity<IdentityUserToken<string>>()
                .ToTable("UserTokens", SqlSchemas.auth);
            builder.Entity<IdentityRole>()
                .ToTable("Roles", SqlSchemas.auth);
            builder.Entity<IdentityRoleClaim<string>>()
                .ToTable("RoleClaims", SqlSchemas.auth);
            #endregion

            #region SeedIdentityRoles

            builder.Entity<IdentityRole>()
                .HasData(
            new IdentityRole<string>
            {
                Id = "d0b1b383-e64c-4f42-b732-7ffbe8f3666b",
                Name = Role.Admin,
                NormalizedName = Role.Admin.ToLower(),
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
            appUser.PasswordHash = passwordHash.HashPassword(appUser, "Icity@2022");

            //seed user
            builder.Entity<User>()
                .HasData(appUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>
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
    }

}
