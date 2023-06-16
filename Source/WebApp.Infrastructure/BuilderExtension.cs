using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Core.Entities.Auth;
using WebApp.Core.Helpers;
using WebApp.Core.Interfaces;
using WebApp.Core.TokenProviders;
using WebApp.Infrastructure.DBContexts;
using WebApp.Infrastructure.Repositories;
using WebApp.SharedKernel.Consts;

namespace WebApp.Infrastructure
{
    public static class BuilderExtension
    {
        public static void BuildInfrastructure(this WebApplicationBuilder builder)
        {

            #region Connection String
            string connectionString = builder.Configuration.GetConnectionString("ConStr")!;
            builder.Services.AddSingleton(new ConnectionStringConfiguration() { ConStr = connectionString });
            #endregion

            #region Add DbContext
            builder.Services.AddDbContext<WebAppDBContext>(options =>
            {
                options.UseLazyLoadingProxies()
                //.ConfigureWarnings(warn => warn.Ignore(CoreEventId.DetachedLazyLoadingWarning))
                .UseSqlServer(connectionString, b =>
                {
                    b.MigrationsAssembly(typeof(WebAppDBContext).Assembly.FullName);
                     //.UseNetTopologySuite()
                });
            });
            #endregion

            #region Add Identity
            builder.Services.AddIdentity<User, Role>(option =>
            {
                //option.Password.RequiredLength = 7
                //option.Password.RequireDigit = false
                //option.Password.RequireUppercase = false
                option.SignIn.RequireConfirmedAccount = true;
                option.SignIn.RequireConfirmedEmail = true;
                option.SignIn.RequireConfirmedPhoneNumber = true;
                option.User.RequireUniqueEmail = false;
                option.Tokens.EmailConfirmationTokenProvider = Res.emailConfirmation;
            }).AddEntityFrameworkStores<WebAppDBContext>()
              .AddDefaultTokenProviders()
              .AddTokenProvider<EmailConfirmationTokenProvider<User>>(Res.emailConfirmation);
            #endregion

            #region HangFire
            builder.Services.AddHangfire(c => c
                            .UseSimpleAssemblyNameTypeSerializer()
                            .UseRecommendedSerializerSettings()
                            .UseSqlServerStorage(connectionString))
                            .AddHangfireServer();
            #endregion

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
