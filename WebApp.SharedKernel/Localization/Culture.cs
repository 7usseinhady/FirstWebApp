using WebApp.SharedKernel.Interfaces;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Collections.Immutable;
using WebApp.SharedKernel.Localization.Resources;
using WebApp.SharedKernel.Localization;

namespace WebApp.SharedKernel.Helpers
{
    public partial class Culture
    {
        // Supported lang (string)
        private static readonly ImmutableArray<string> _supportedCultures = ImmutableArray.Create(
            string.Concat(ELanguages.ar.ToString(), "-EG"),
            ELanguages.en.ToString());

        // Default Lang (Enum)
        public static readonly ELanguages Default = ELanguages.ar;


        public static readonly ImmutableArray<CultureInfo> _supportedCulturesInfo = _supportedCultures.Select(x => new CultureInfo(x)).ToImmutableArray();
        public static readonly CultureInfo DefaultCultureInfo = _supportedCulturesInfo.FirstOrDefault(x => x.Name.StartsWith(Default.ToString()))!;

        public static ELanguages GetLanguage(string cultureName)
        {
            switch (cultureName)
            {
                case string s when s.StartsWith(ELanguages.ar.ToString()):
                    return ELanguages.ar;

                case string s when s.StartsWith(ELanguages.en.ToString()):
                    return ELanguages.en;

                default:
                    return Default;
            }
        }

        public static ELanguages CurrentLanguage()
        {
            return GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
        }
    }

    public partial class Culture : ICulture
    {
        public IStringLocalizer<SharedResource> SharedLocalizer { get; private set; }

        public Culture(IStringLocalizer<SharedResource> SharedLocalizer)
        {
            this.SharedLocalizer = SharedLocalizer;
        }
    }
}
