using Microsoft.Extensions.DependencyInjection;
using WebApp.DataTransferObject.Dtos;
using WebApp.DataTransferObject.Interfaces;

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
