using Microsoft.Extensions.Options;
using Hangfire;
using WebApp.SharedKernel;
using WebApp.Core;
using WebApp.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using WebApp.API.Middlewares;
using Microsoft.AspNetCore.Mvc.Versioning;
using WebApp.API.ServiceConfigurations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Build WebApp builder Load Services and IOC Containers for other Projects
builder.BuildSharedKernel();
builder.BuildCore();
builder.BuildInfrastructure();
#endregion

#region API .Net Core IOC Container


#endregion

#region timeOutResponse
builder.WebHost.ConfigureKestrel(c =>
{
    c.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
});
#endregion

#region Json newtonsoft and json patch
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
#endregion

#region Api Versioning

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader()
        //new QueryStringApiVersionReader(ApiVersions.apiVersion),
        //new HeaderApiVersionReader(ApiVersions.apiVersion),
        //new MediaTypeApiVersionReader(ApiVersions.apiVersion)
        );
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// to let swagger create swagger json files for other versions
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddEndpointsApiExplorer();

#endregion

#region Swagger

builder.Services.AddSwaggerGen(c =>
{
    //Add the custom header to all operations
    //c.OperationFilter<OperationFilter>()

    // setup Bearer Authorization Field
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


#endregion

#region Add Cors
builder.Services.AddCors();
#endregion



var app = builder.Build();

app.UseSwagger();
IEnumerable<string> groupNames = app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions.Select(x => x.GroupName);
app.UseSwaggerUI(options =>
{
    foreach (var groupName in groupNames)
    {
        options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
    }
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

//app.UseAuthKeyBasedMiddleware()

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire/dashboard", new DashboardOptions()
{
    DashboardTitle = "Hangfire Dashboard",
    Authorization = new[] { new HangFireAuthorizationFilter() }
});
app.MapControllers();

app.Run();
