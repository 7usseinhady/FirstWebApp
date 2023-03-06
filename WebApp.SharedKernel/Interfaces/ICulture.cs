using WebApp.SharedKernel.Resources;
using Microsoft.Extensions.Localization;

namespace WebApp.SharedKernel.Interfaces
{
    public interface ICulture
    {
        IStringLocalizer<SharedResource> SharedLocalizer { get;}
    }
}
