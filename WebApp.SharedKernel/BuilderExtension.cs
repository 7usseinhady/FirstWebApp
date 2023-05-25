using Microsoft.Extensions.DependencyInjection;
using WebApp.SharedKernel.Helpers;
using WebApp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using WebApp.SharedKernel.Dtos;
using WebApp.SharedKernel.Localization.Resources;
using System.Globalization;

namespace WebApp.SharedKernel
{
    public static class BuilderExtension
    {
        public static void BuildSharedKernel(this WebApplicationBuilder builder)
        {

            #region localization and globalization
            builder.Services.AddLocalization();

            builder.Services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(DataAnnotationResource));
                });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(Culture.DefaultCultureInfo, Culture.DefaultCultureInfo);
                options.SupportedCultures = Culture._supportedCulturesInfo;
                options.SupportedUICultures = Culture._supportedCulturesInfo;
                options.RequestCultureProviders = new List<IRequestCultureProvider>()
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });
            #endregion

            #region for baseApiConnection
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();
            #endregion

            builder.Services.AddTransient<HolderOfDto>();
            builder.Services.AddTransient<Indicator>();

            builder.Services.AddScoped<ICulture, Culture>();

            builder.Services.AddScoped<IBaseApiConnection, BaseApiConnection>();
            builder.Services.AddScoped<IFileUtils, FileUtils>();
        }
    }
}
