using System.IdentityModel.Tokens.Jwt;
using AuthorizationServer.Data;
using AuthorizationServer.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
/*
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("51.210.99.16"));
});
*/
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

if (args.Contains("/seed"))
{
    Log.Information("Seeding database...");
    SeedData.EnsureSeedData(app);
    Log.Information("Done seeding database. Exiting");
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();
        
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();