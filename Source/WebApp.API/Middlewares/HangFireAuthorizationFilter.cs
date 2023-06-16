using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace WebApp.API.Middlewares
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            //Can add some more logic here...

            return true;
        }
    }
}
