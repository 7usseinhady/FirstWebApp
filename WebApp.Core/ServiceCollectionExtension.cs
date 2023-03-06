using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Core.Helpers.AutoMapper;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.Core.Services.Auth;
using WebApp.Core.Services;

namespace WebApp.Core
{
    public static class ServiceCollectionExtension
    {
        public static void LoadCoreServices(this IServiceCollection services)
        {
            #region autoMapper
            services.AddAutoMapper(typeof(ServiceCollectionExtension));
            services.AddSingleton<SharedMapper>();
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile(provider.GetService<SharedMapper>()!));
            }).CreateMapper());
            #endregion

            #region Auth
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            #endregion
        }
    }
}
