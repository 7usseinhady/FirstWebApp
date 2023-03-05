using System.Security.Claims;
using WebApp.SharedKernel.Consts;

namespace WebApp.SharedKernel.Extensions
{
    public static class ContextUserDataExtension
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue(Res.uid)!;
        }
    }
}
