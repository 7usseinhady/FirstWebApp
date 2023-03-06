using Microsoft.Extensions.DependencyInjection;
using WebApp.DataTransferObject.Dtos;
using WebApp.DataTransferObjects.Interfaces;

namespace WebApp.DataTransferObject
{
    public static class ServiceCollectionExtension
    {
        public static void LoadDataTransferObjectServices(this IServiceCollection services)
        {
            services.AddTransient<HolderOfDto>();
        }
    }
}
