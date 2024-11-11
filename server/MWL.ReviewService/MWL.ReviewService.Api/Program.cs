using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using MWL.ReviewService.Api.Helpers;
using MWL.ReviewService.Api.Mappers;
using MWL.ReviewService.Application.Services;
using MWL.ReviewService.Domain.Repositories;
using MWL.ReviewService.Domain.Services;
using MWL.ReviewService.Messaging;
using MWL.ReviewService.Storage.DbContext;
using MWL.ReviewService.Storage.Repositories;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<RPCServerManager>();

builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<IReviewRepository, ReviewRepository>();

builder.Services.AddAutoMapper(typeof(ReviewMapper));

if (builder.Environment.IsEnvironment("Testing"))
{
    Console.WriteLine("Testing Environment");
    builder.Services.AddDbContext<ReviewContext>(options =>
    {
        options.UseInMemoryDatabase("Test");
    });
}
else if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("Development Environment");
    builder.Services.AddDbContext<ReviewContext>(options =>
    {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("TestUrl"),
            x => { x.MigrationsAssembly("MWL.ReviewService.Api"); });
    });
}
else if (builder.Environment.IsProduction())
{
    Console.WriteLine("Production Environment");
    //var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
    var connectionString = builder.Configuration.GetConnectionString("Azure");
    //const string secretName_host = "reviewservicedb-host";
    //const string secretName_database = "reviewservicedb-database";
    //const string secretName_user = "reviewservicedb-user";
    //const string secretName_password = "reviewservicedb-password";

    //string keyVaultName = "kv-individual-gwc";
    //string keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";

    //var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

    //var host = await client.GetSecretAsync(secretName_host);
    //var database = await client.GetSecretAsync(secretName_database);
    //var user = await client.GetSecretAsync(secretName_user);
    //var password = await client.GetSecretAsync(secretName_password);

    //var connectionString = $"Host={host.Value.Value};Database={database.Value.Value};Username={user.Value.Value};Password={password.Value.Value};Ssl Mode=Require;";

    Console.WriteLine($"Connection String: {connectionString}");
    builder.Services.AddDbContext<ReviewContext>(options =>
    {
        options.UseNpgsql(
                       connectionString,
                                  x => { x.MigrationsAssembly("MWL.ReviewService.Api"); });
    });
}

//builder.Services.AddDbContext<ReviewContext>(options =>
//{
//    options.UseNpgsql(
//        builder.Configuration.GetConnectionString("Url"),
//        x => { x.MigrationsAssembly("MWL.ReviewService.Api"); });
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
            var context = scope.ServiceProvider.GetRequiredService<ReviewContext>();
            context.Database.Migrate();
        }, 5, TimeSpan.FromSeconds(10));
    }
}

app.UseHttpsRedirection();

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
