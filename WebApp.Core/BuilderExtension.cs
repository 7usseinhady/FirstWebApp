using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Core.Helpers.AutoMapper;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.Core.Services.Auth;
using WebApp.Core.Services;
using WebApp.Core.Helpers.Email.MailKit;
using WebApp.Core.Helpers.Notification.FCM;
using WebApp.Core.Helpers.Payment.PayTabs.Dtos.Request;
using WebApp.Core.Helpers.Payment.PayTabs;
using WebApp.Core.Helpers.Sms.GatewaySms;
using WebApp.Core.Interfaces;
using StackExchange.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApp.Core.Helpers.Email.SendGrid;
using WebApp.Core.Helpers.Sms.TwilioSms;
using WebApp.SharedKernel.Consts;
using Microsoft.Extensions.Configuration;
using WebApp.Core.TokenProviders;

namespace WebApp.Core
{
    public static class BuilderExtension
    {

        public static void BuildCore(this WebApplicationBuilder builder)
        {
            #region Email SetUp
            // MailKit email configration
            var mailKitEmailConfig = builder.Configuration
                                            .GetSection(Res.MailKitEmailConfiguration)
                                            .Get<MailKitEmailConfiguration>();
            builder.Services.AddSingleton(mailKitEmailConfig!);

            // SendGrid email configration
            var sendGridEmailConfig = builder.Configuration
                                     .GetSection(Res.SendGridEmailConfiguration)
                                     .Get<SendGridKeyConfiguration>();
            builder.Services.AddSingleton(sendGridEmailConfig!);

            // email confirmation
            builder.Services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
                           opt.TokenLifespan = TimeSpan.FromHours(2));
            #endregion

            #region Sms SetUp
            // Twilio Sms configration
            var twilioSmsConfiguration = builder.Configuration
                                            .GetSection(Res.TwilioSmsConfiguration)
                                            .Get<TwilioSmsConfiguration>();
            builder.Services.AddSingleton(twilioSmsConfiguration!);

            // Gateway Sms configration
            var gateWaySmsConfiguration = builder.Configuration
                                            .GetSection(Res.GatewaySmsConfiguration)
                                            .Get<GatewaySmsConfiguration>();
            builder.Services.AddSingleton(gateWaySmsConfiguration!);
            #endregion

            #region JWT Auth
            builder.Services.Configure<WebApp.Core.Helpers.Jwt>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
                        ClockSkew = TimeSpan.FromMinutes(30)
                    };
                });

            #endregion

            #region autoMapper
            builder.Services.AddAutoMapper(typeof(BuilderExtension));
            builder.Services.AddSingleton<SharedMapper>();
            builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile(provider.GetService<SharedMapper>()!));
            }).CreateMapper());
            #endregion

            #region Auth
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();
            #endregion

            #region for Base Api Connection SetUp
            // PayTabs
            builder.Services.AddHttpClient(Res.PayTabsUri, c =>
            {
                string PayTabsUri = builder.Configuration.GetValue<string>(Res.PayTabsUri)!;
                if (!string.IsNullOrEmpty(PayTabsUri))
                    c.BaseAddress = new Uri(PayTabsUri);

                string payTabsServerToken = builder.Configuration.GetValue<string>(Res.payTabsServerToken)!;
                if (!string.IsNullOrEmpty(payTabsServerToken))
                    c.DefaultRequestHeaders.Add("Authorization", payTabsServerToken);
            });

            // GateWaySms
            builder.Services.AddHttpClient(Res.GatewaySmsUri, c =>
            {
                string GateWaySmsUri = builder.Configuration.GetValue<string>(Res.GatewaySmsUri)!;
                if (!string.IsNullOrEmpty(GateWaySmsUri))
                    c.BaseAddress = new Uri(GateWaySmsUri);
            });
            #endregion


            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddScoped<IEmailSender, MailKitEmailSender>();
            //Service.AddScoped<IEmailSender, SendGridEmailSender>()

            builder.Services.AddScoped<ISmsService, GatewaySmsService>();
            //Service.AddScoped<ISmsService, TwilioSmsService>()

            builder.Services.AddScoped<IPaymentService<PaymentInitialRequestDto>, PayTabsPaymentService>();

            builder.Services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(builder.Configuration["redisConnections"]!));
        }
    }
}
