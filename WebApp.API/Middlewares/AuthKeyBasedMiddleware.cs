using WebApp.SharedKernel.Consts;

namespace WebApp.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthKeyBasedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthKeyBasedMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var secretKey = _configuration.GetValue<string>(Res.secretKey);
                var requestObject = httpContext.Request.Headers.FirstOrDefault(x => x.Key == Res.secretKey).Value.FirstOrDefault();
                if (!string.IsNullOrEmpty(requestObject) && requestObject == secretKey)
                    await _next(httpContext);
                else
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("Forbidden Access");
                }
            }
            catch
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Forbidden Access");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthKeyBasedMiddleware1Extensions
    {
        public static IApplicationBuilder UseAuthKeyBasedMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthKeyBasedMiddleware>();
        }
    }
}
