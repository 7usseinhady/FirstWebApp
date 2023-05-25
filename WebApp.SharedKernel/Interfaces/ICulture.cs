using Microsoft.Extensions.Localization;
using WebApp.SharedKernel.Localization.Resources;

namespace WebApp.SharedKernel.Interfaces
{
    public interface ICulture
    {
        IStringLocalizer<SharedResource> SharedLocalizer { get;}
    }
}
