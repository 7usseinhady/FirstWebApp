using Microsoft.Extensions.DependencyInjection;
using WebApp.SharedKernel.Helpers;
using WebApp.SharedKernel.Interfaces;
using WebApp.SharedKernel.Helpers.Payment.PayTabs;
using WebApp.SharedKernel.Helpers.Notification.FCM;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using WebApp.SharedKernel.Resources;
using WebApp.SharedKernel.Helpers.Email.MailKit;
using WebApp.SharedKernel.Helpers.Payment.PayTabs.Dtos.Request;
using WebApp.SharedKernel.Helpers.Sms.GatewaySms;

namespace WebApp.SharedKernel
{
    public static class ServiceCollectionExtension
    {
        public static void LoadSharedKernelServices(this IServiceCollection services)
        {
            #region localization and globalization
            services.AddLocalization();

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(DataAnnotationResource));
                });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCulturesInfo = Culture.GetSupportedCulturesInfo();
                options.DefaultRequestCulture = new RequestCulture(supportedCulturesInfo[0], supportedCulturesInfo[0]);
                options.SupportedCultures = supportedCulturesInfo;
                options.SupportedUICultures = supportedCulturesInfo;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider()
        };
            });
            #endregion

            #region for baseApiConnection
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            #endregion

            services.AddScoped<ICulture, Culture>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IEmailSender, MailKitEmailSender>();
            //Service.AddScoped<IEmailSender, SendGridEmailSender>();

            services.AddScoped<ISmsService, GatewaySmsService>();
            //Service.AddScoped<ISmsService, TwilioSmsService>();

            services.AddScoped<IPaymentService<PaymentInitialRequestDto>, PayTabsPaymentService>();
            services.AddScoped<IBaseApiConnection, BaseApiConnection>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IFileUtils, FileUtils>();
        }
    }
}
