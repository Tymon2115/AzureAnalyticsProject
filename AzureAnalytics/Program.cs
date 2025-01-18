using AzureAnalytics.Data;
using AzureAnalytics.Models;
using AzureAnalytics.Repositories;
using AzureAnalytics.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureDevOpsConfig>(options =>
{
    options.Organization = Environment.GetEnvironmentVariable("AZURE_DEVOPS_ORG");
    options.PersonalAccessToken = Environment.GetEnvironmentVariable("AZURE_DEVOPS_PAT");
    options.Project = Environment.GetEnvironmentVariable("AZURE_DEVOPS_PROJECT");
});

var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("SQL connection string is missing. Check your environment variables.");
}

builder.Services.AddDbContext<StatisticsDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddHttpClient<AzureDevOpsService>();

builder.Services.AddTransient<AzureDevOpsService>();

builder.Services.AddHostedService<StatisticsCollector>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
