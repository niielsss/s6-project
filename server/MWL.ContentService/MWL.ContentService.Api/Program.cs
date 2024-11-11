using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using MWL.ContentService.Api.Filters;
using MWL.ContentService.Api.Helpers;
using MWL.ContentService.Api.Mappers;
using MWL.ContentService.Application.Services;
using MWL.ContentService.Domain.Repositories;
using MWL.ContentService.Domain.Services;
using MWL.ContentService.Storage.DbContext;
using MWL.ContentService.Storage.Repositories;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(x => x.Filters.Add<ExceptionFilter>());
builder.Services.AddProblemDetails();
builder.Services.AddTransient<IMovieService, MovieService>();
builder.Services.AddTransient<IMovieRepository, MovieRepository>();
builder.Services.AddAutoMapper(typeof(MovieMapper));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsEnvironment("Testing"))
{
    Console.WriteLine("Testing Environment");
    builder.Services.AddDbContext<ContentContext>(options =>
    {
        options.UseInMemoryDatabase("Test");
    });
}
else if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("Development Environment");
    //const string secretName_host = "contentservicedb-host";
    //const string secretName_database = "contentservicedb-database";
    //const string secretName_user = "contentservicedb-user";
    //const string secretName_password = "contentservicedb-password";

    //string keyVaultName = "kv-individual-gwc";
    //string keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";

    //var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

    //var host = await client.GetSecretAsync(secretName_host);
    //var database = await client.GetSecretAsync(secretName_database);
    //var user = await client.GetSecretAsync(secretName_user);
    //var password = await client.GetSecretAsync(secretName_password);

    //var connectionString = $"Host={host.Value.Value};Database={database.Value.Value};Username={user.Value.Value};Password={password.Value.Value};Ssl Mode=Require;";

    //Console.WriteLine($"Connection String: {connectionString}");


    builder.Services.AddDbContext<ContentContext>(options =>
    {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("Test"),
            //connectionString,
            x => { x.MigrationsAssembly("MWL.ContentService.Api"); });
    });
}
else if (builder.Environment.IsProduction())
{
    Console.WriteLine("Production Environment");
    //string connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
    string connectionString = builder.Configuration.GetConnectionString("Azure");

    //const string secretName_host = "contentservicedb-host";
    //const string secretName_database = "contentservicedb-database";
    //const string secretName_user = "contentservicedb-user";
    //const string secretName_password = "contentservicedb-password";

    //string keyVaultName = "kv-individual-gwc";
    //string keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";

    //var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

    //var host = await client.GetSecretAsync(secretName_host);
    //var database = await client.GetSecretAsync(secretName_database);
    //var user = await client.GetSecretAsync(secretName_user);
    //var password = await client.GetSecretAsync(secretName_password);

    //var connectionString = $"Host={host.Value.Value};Database={database.Value.Value};Username={user.Value.Value};Password={password.Value.Value};Ssl Mode=Require;";

    Console.WriteLine($"Connection String: {connectionString}");

    builder.Services.AddDbContext<ContentContext>(options =>
    {
        options.UseNpgsql(
                       connectionString,
                                  x => { x.MigrationsAssembly("MWL.ContentService.Api"); });
    });
}


//builder.Services.AddDbContext<ContentContext>(options =>
//{
//    options.UseNpgsql(
//        builder.Configuration.GetConnectionString("Url"),
//        x => { x.MigrationsAssembly("MWL.ContentService.Api"); });
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
            var context = scope.ServiceProvider.GetRequiredService<ContentContext>();
            context.Database.Migrate();
        }, 5, TimeSpan.FromSeconds(10));
    }
}

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

app.UseAuthorization();

app.MapControllers();

app.Run();