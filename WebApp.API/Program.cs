using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.Core.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Helpers.Email.SendGrid;
using WebApp.SharedKernel.Helpers.Sms.GatewaySms;
using WebApp.SharedKernel.Helpers.Sms.TwilioSms;
using WebApp.SharedKernel;
using WebApp.DataTransferObject;
using WebApp.Core;
using WebApp.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using WebApp.API.Middlewares;
using WebApp.SharedKernel.Helpers.Email.MailKit;
using WebApp.Infrastructure.TokenProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region timeOutResponse
builder.WebHost.ConfigureKestrel(c =>
{
    c.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
});
#endregion

#region Json newtonsoft and json patch
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
#endregion

#region Connection String
string conStr = builder.Configuration.GetConnectionString("ConStr")!;
builder.Services.AddSingleton(new ConnectionStringConfiguration() { ConStr = conStr });
#endregion

#region HangFire
builder.Services.AddHangfire(c => c
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(conStr))
                .AddHangfireServer();
#endregion

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

#region Load Services and IOC Containers for other Projects
builder.Services.LoadSharedKernelServices();
builder.Services.LoadDataTransferObjectServices();
builder.Services.LoadCoreServices();
builder.Services.LoadInfrastructureServices(conStr);
#endregion

#region API .Net Core IOC Container
builder.Services.AddSingleton<AuthKeyBasedMiddleware>();
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

#region Add Cors
builder.Services.AddCors();
#endregion



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      Array.Empty<string>()
    }
  });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Local");
    options.SwaggerEndpoint(url: "./swagger/v1/swagger.json", name: "IIS");
});

app.UseHttpsRedirection();

app.UseRouting();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions!.Value);

// app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())

// app.UseMiddleware<AuthKeyBasedMiddleware>()

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire/dashboard", new DashboardOptions()
{
    DashboardTitle = "Hangfire Dashboard",
    Authorization = new[] { new HangFireAuthorizationFilter() }
});
app.MapControllers();

app.Run();
