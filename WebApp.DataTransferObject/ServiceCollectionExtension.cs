using Microsoft.Extensions.DependencyInjection;
using WebApp.DataTransferObjects.Helpers;
using WebApp.DataTransferObjects.Interfaces;

namespace WebApp.DataTransferObject
{
    public static class ServiceCollectionExtension
    {
        public static void LoadDataTransferObjectServices(this IServiceCollection services)
        {
            services.AddTransient<HolderOfDTO>();
        }
    }
}
