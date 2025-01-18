namespace AzureAnalytics.Services
{
    public class StatisticsCollector : BackgroundService
    {
        private readonly AzureDevOpsService _azureDevOpsService;

        private readonly IServiceProvider _serviceProvider;

        public StatisticsCollector(AzureDevOpsService azureDevOpsService, IServiceProvider serviceProvider)
        {
            _azureDevOpsService = azureDevOpsService;
            _serviceProvider = serviceProvider;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {

                    try
                    {
                        var statisticsService = scope.ServiceProvider.GetRequiredService<IStatisticsService>();

                        Console.WriteLine($"[{DateTime.Now}] Collecting statistics from Azure DevOps...");

                        var workItemBatchReponse = await _azureDevOpsService.GetWorkItemStatisticsAsync();
                        await statisticsService.AddAsync(workItemBatchReponse);

                        Console.WriteLine($"[{DateTime.Now}] Data collection completed.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during data collection: {ex.Message}");
                    }

                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }
    }
}
