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

var config = new AzureDevOpsConfig
{
    Organization = Environment.GetEnvironmentVariable("AZURE_DEVOPS_ORG"),
    PersonalAccessToken = Environment.GetEnvironmentVariable("AZURE_DEVOPS_PAT"),
    Project = Environment.GetEnvironmentVariable("AZURE_DEVOPS_PROJECT")
};

if (string.IsNullOrWhiteSpace(config.Organization) ||
    string.IsNullOrWhiteSpace(config.PersonalAccessToken) ||
    string.IsNullOrWhiteSpace(config.Project))
{
    Console.WriteLine("Error: Missing Azure DevOps environment variables:");
    Console.WriteLine($"AZURE_DEVOPS_ORG: {config.Organization}");
    Console.WriteLine($"AZURE_DEVOPS_PAT: {config.PersonalAccessToken}");
    Console.WriteLine($"AZURE_DEVOPS_PROJECT: {config.Project}");
    throw new InvalidOperationException("Azure DevOps configuration is missing or incomplete.");
}


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

// Register the background service
builder.Services.AddHostedService<StatisticsCollector>();

// Add controllers (API endpoints)
builder.Services.AddControllers();

var app = builder.Build();

// Map API routes
app.MapControllers();

app.Run();
