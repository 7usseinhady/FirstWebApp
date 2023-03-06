using WebApp.SharedKernel.Consts;

namespace WebApp.API.Middlewares
{
    public class AuthKeyBasedMiddleware : IMiddleware
    {
        private readonly IConfiguration _configuration;
        public AuthKeyBasedMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var secretKey = _configuration.GetValue<string>(Res.SecretKey);
                var requestObject = context.Request.Headers.FirstOrDefault(x => x.Key == Res.SecretKey).Value.FirstOrDefault();
                if (!string.IsNullOrEmpty(requestObject) && requestObject == secretKey)
                    await next(context);
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Forbidden Access");
                }
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Forbidden Access");
            }
        }
    }
}
