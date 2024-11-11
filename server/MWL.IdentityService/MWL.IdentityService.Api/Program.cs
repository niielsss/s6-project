using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MWL.IdentityService.Api.Helpers;
using MWL.IdentityService.Application.Services;
using MWL.IdentityService.Domain.Entities;
using MWL.IdentityService.Domain.Services;
using MWL.IdentityService.Storage.DbContext;
using Prometheus;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database

if (builder.Environment.IsEnvironment("Testing"))
{
    Console.WriteLine("Testing Environment");
    builder.Services.AddDbContext<IdentityContext>(options =>
    {
        options.UseInMemoryDatabase("Test");
    });
}
else if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("Development Environment");
    builder.Services.AddDbContext<IdentityContext>(options =>
    {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("Test"),
            x => { x.MigrationsAssembly("MWL.IdentityService.Api"); });
    });
}
else if (builder.Environment.IsProduction())
{
    Console.WriteLine("Production Environment");
    //string connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
    string connectionString = builder.Configuration.GetConnectionString("Azure");
    //const string secretName_host = "identityservicedb-host";
    //const string secretName_database = "identityservicedb-database";
    //const string secretName_user = "identityservicedb-user";
    //const string secretName_password = "identityservicedb-password";

    //string keyVaultName = "kv-individual-gwc";
    //string keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";

    //var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

    //var host = await client.GetSecretAsync(secretName_host);
    //var database = await client.GetSecretAsync(secretName_database);
    //var user = await client.GetSecretAsync(secretName_user);
    //var password = await client.GetSecretAsync(secretName_password);

    //var connectionString = $"Host={host.Value.Value};Database={database.Value.Value};Username={user.Value.Value};Password={password.Value.Value};Ssl Mode=Require;";

    Console.WriteLine($"Connection String: {connectionString}");
    builder.Services.AddDbContext<IdentityContext>(options =>
    {
        options.UseNpgsql(
                       connectionString,
                                  x => { x.MigrationsAssembly("MWL.IdentityService.Api"); });
    });
}


// Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

// Cookie
//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.MinimumSameSitePolicy = SameSiteMode.Strict;
//    options.HttpOnly = HttpOnlyPolicy.Always;
//    options.Secure = CookieSecurePolicy.Always;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        RetryHelper.Retry(() =>
        {
            var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            context.Database.Migrate();
        }, 5, TimeSpan.FromSeconds(10));
    }
}

//app.MapIdentityApi<User>();

app.UseHttpsRedirection();

app.UseMetricServer();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpMetrics(options =>
{
    options.AddCustomLabel("host", context => context.Request.Host.Host);
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
