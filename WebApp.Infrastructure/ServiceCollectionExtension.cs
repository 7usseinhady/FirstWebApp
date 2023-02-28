using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Core.Entities.Auth;
using WebApp.Infrastructure.DBContexts;
using WebApp.Infrastructure.TokenProviders;
using WebApp.SharedKernel.Consts;

namespace WebApp.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static void LoadInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            #region Add DbContext
            services.AddDbContext<WebAppDBContext>(options =>
            {
                options.UseLazyLoadingProxies()
                //.ConfigureWarnings(warn => warn.Ignore(CoreEventId.DetachedLazyLoadingWarning))
                .UseSqlServer(connectionString, b =>
                {
                    b.MigrationsAssembly(typeof(WebAppDBContext).Assembly.FullName);
                     //.UseNetTopologySuite();
                });
            });
            #endregion

            #region Add Identity
            services.AddIdentity<User, IdentityRole>(option =>
            {
                //option.Password.RequiredLength = 7;
                //option.Password.RequireDigit = false;
                //option.Password.RequireUppercase = false;
                option.SignIn.RequireConfirmedAccount = true;
                option.SignIn.RequireConfirmedEmail = true;
                option.SignIn.RequireConfirmedPhoneNumber = true;
                option.User.RequireUniqueEmail = false;
                option.Tokens.EmailConfirmationTokenProvider = Res.EmailConfirmation;
            }).AddEntityFrameworkStores<WebAppDBContext>()
              .AddDefaultTokenProviders()
              .AddTokenProvider<EmailConfirmationTokenProvider<User>>(Res.EmailConfirmation);
            #endregion
        }
    }
}
