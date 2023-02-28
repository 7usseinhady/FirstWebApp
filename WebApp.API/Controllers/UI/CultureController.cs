using WebApp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.API.Controllers.UI
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Controller]
    [Route("[controller]")]
    public class CultureController : Controller
    {
        protected readonly ICulture _culture;
        public CultureController(ICulture culture)
        {
            _culture = culture;
        }

        
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
    }
}
