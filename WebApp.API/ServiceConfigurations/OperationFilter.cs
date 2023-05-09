using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApp.API.ServiceConfigurations
{
    public class OperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            #region Add Custom Paramerters

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "Language",
                In = ParameterLocation.Header,
                Description = "Custom Header",
                Required = false // set to true if this header is mandatory
            });

            #endregion
        }

    }
}
