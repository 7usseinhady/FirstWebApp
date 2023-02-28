using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Core.Helpers.AutoMapper;

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
                cfg.AddProfile(new MappingProfile(provider.GetService<SharedMapper>()));
            }).CreateMapper());
            #endregion
        }
    }
}
