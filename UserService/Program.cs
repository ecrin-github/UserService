using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using UserService.Configs;
using UserService.Extensions;
using UserService.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("51.210.99.16"));
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc($"{ApiConfigs.ApiVersion}", 
        new OpenApiInfo { Title = "The User API Documentation", Version = $"{ApiConfigs.ApiVersion}" });
    c.EnableAnnotations();
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    };
    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(ApiConfigs.OpenCorsPolicyName, p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.WebHost.UseUrls("http://localhost:7220");

// Docker setting
// builder.WebHost.UseKestrel(options => {options.Listen(IPAddress.Any, 80);});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "The User API Documentation";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "The User API Documentation (v.1)");
    c.InjectStylesheet("/documentation/swagger-custom/swagger-custom-styles.css");
    c.InjectJavascript("/documentation/swagger-custom/swagger-custom-script.js");
    c.RoutePrefix = "api/rest/documentation";
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(ApiConfigs.OpenCorsPolicyName);

app.MapControllers();

app.Run();