using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace WebApp.SharedKernel.Helpers
{
    public static class RequestUtils
    {
        public static CultureInfo GetRequestCulureInfo(HttpContext? httpContext)
        {
            if (httpContext is null)
                return null!;
            return httpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture;
        }

        public static string GetClientIPAddress(HttpContext? httpContext)
        {
            if (httpContext is null)
                return null!;
            string clientIp = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            return clientIp;
        }

        public static string GetClientAgent(HttpContext? httpContext)
        {
            if (httpContext is null)
                return null!;
            string clientAgent = string.Empty;
            if (!string.IsNullOrEmpty(httpContext.Request.Headers["User-Agent"]))
                clientAgent = httpContext.Request.Headers["User-Agent"];
            return clientAgent;
        }

        public static void SetCookie(HttpContext httpContext, string key, string value, CookieOptions cookieOptions)
        {
            httpContext.Response.Cookies.Append(key, value, cookieOptions);
        }

        public static string GetCookie(HttpContext httpContext, string key)
        {
            return httpContext.Request.Cookies[key];
        }

        public static void DeleteCookie(HttpContext httpContext, string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }

    }
}
