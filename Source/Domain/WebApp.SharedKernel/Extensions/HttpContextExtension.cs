using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApp.SharedKernel.Consts;

namespace WebApp.SharedKernel.Extensions
{
    public static class HttpContextExtension
    {
        private const string AuthorizationHeaderKey = "Authorization";
        private const string Schema = "Bearer ";
        private const string Algorithm = "HS256";

        public static JwtSecurityToken? GetJwtSecurityToken(this HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers[AuthorizationHeaderKey].FirstOrDefault();

            if (authorizationHeader == null || !authorizationHeader.StartsWith(Schema))
            {
                // Authorization header is missing or invalid
                return null;
            }

            var token = authorizationHeader.Substring(Schema.Length);
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            if (jwtToken is null || !jwtToken.Header.Alg.Equals(Algorithm))
                return null;

            return jwtToken;
        }
        
        public static string? GetUserId(this HttpContext httpContext)
        {
            var jwtToken = GetJwtSecurityToken(httpContext);

            if (jwtToken is null)
                return null;

            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == Res.uid);

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                return null;

            return userIdClaim.Value;
        }
    }
}
