using IdentityServer.Configs;
using IdentityServer.Extensions;
using IdentityServer.Middleware;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("51.210.99.16"));
});
*/

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy(ApiConfigs.OpenCorsPolicyName, p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// builder.WebHost.UseUrls("https://localhost:5270");

// Docker setting
// builder.WebHost.UseKestrel(options => {options.Listen(IPAddress.Any, 80);});

var app = builder.Build();

/*
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
*/

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.UseCors(ApiConfigs.OpenCorsPolicyName);

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();