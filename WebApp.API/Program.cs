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
using WebApp.SharedKernel.Helpers.SMS.GatewaySMS;
using WebApp.SharedKernel.Helpers.SMS.TwilioSMS;
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
//builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddControllersWithViews(options =>
{
    options.InputFormatters.Insert(0, new ServiceCollection()
        .AddLogging()
        .AddMvc()
        .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
        //.AddNewtonsoftJson()
        .Services.BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>()
        .Value
        .InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>().First());
});
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
// MimeKit email configration
var mimeKitEmailConfig = builder.Configuration
                                .GetSection(Res.MailKitEmailConfiguration)
                                .Get<MailKitEmailConfiguration>();
builder.Services.AddSingleton(mimeKitEmailConfig);

// SendGrid email configration
var sendGridEmailConfig = builder.Configuration
                         .GetSection(Res.SendGridEmailConfiguration)
                         .Get<SendGridKeyConfiguration>();
builder.Services.AddSingleton(sendGridEmailConfig);

// email confirmation
builder.Services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
               opt.TokenLifespan = TimeSpan.FromHours(2));
#endregion

#region SMS SetUp
// Twilio SMS configration
var twilioSMSConfiguration = builder.Configuration
                                .GetSection(Res.TwilioSMSConfiguration)
                                .Get<TwilioSMSConfiguration>();
builder.Services.AddSingleton(twilioSMSConfiguration);

// Gateway SMS configration
var gateWaySMSConfiguration = builder.Configuration
                                .GetSection(Res.GatewaySMSConfiguration)
                                .Get<GatewaySMSConfiguration>();
builder.Services.AddSingleton(gateWaySMSConfiguration);
#endregion

#region JWT Auth
builder.Services.Configure<WebApp.Core.Helpers.JWT>(builder.Configuration.GetSection("JWT"));
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
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

// GateWaySMS
builder.Services.AddHttpClient(Res.GatewaySMSUri, c =>
{
    string GateWaySMSUri = builder.Configuration.GetValue<string>(Res.GatewaySMSUri)!;
    if (!string.IsNullOrEmpty(GateWaySMSUri))
        c.BaseAddress = new Uri(GateWaySMSUri);
});
#endregion

#region Add Cors
builder.Services.AddCors();
#endregion



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
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
      new string[] { }
    }
  });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

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
app.UseRequestLocalization(locOptions.Value);

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

//app.UseMiddleware<AuthKeyBasedMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire/dashboard", new DashboardOptions()
{
    DashboardTitle = "Hangfire Dashboard",
    Authorization = new[] { new HangFireAuthorizationFilter() }
});

//app.UseHangfireDashboard("/hangfire", new DashboardOptions()
//{
//    DashboardTitle = "Hangfire Dashboard",
//    Authorization = new[] {
//        new HangfireCustomBasicAuthenticationFilter() {
//            User = builder.Configuration.GetSection("HangfireCredentials:UserName").Value,
//            Pass = builder.Configuration.GetSection("HangfireCredentials:Password").Value
//        }
//    }
//});

// default path /hangfire
//app.MapHangfireDashboard("/hangfire", new DashboardOptions()
//{
//    DashboardTitle = "Hangfire Dashboard",
//    Authorization = new[] { new HangFireAuthorizationFilter() }
//});

app.MapControllers();

app.Run();
