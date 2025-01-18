using AzureAnalytics.Models;
using AzureAnalytics.Repositories;

namespace AzureAnalytics.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository<StatisticEntry> _repository;

        public StatisticsService(IRepository<StatisticEntry> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(WorkItemBatchResponse batchReponse)
        {
            var entry = ProcessWorkItems(batchReponse);
            await _repository.AddAsync(entry);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<StatisticEntry>> GetStatisticsAsync() => await _repository.GetAllAsync();

        private StatisticEntry ProcessWorkItems(WorkItemBatchResponse workItemBatchResponse)
        {
            var statistics = workItemBatchResponse.WorkItems
                   .GroupBy(wi => wi.Fields.Type)
                   .ToDictionary(g => g.Key, g => g.Count());

            var bugsCount = statistics.GetValueOrDefault("Bug", 0);
            var featureCount = statistics.GetValueOrDefault("Feature", 0);

            var entry = new StatisticEntry
            {
                Date = DateTime.UtcNow,
                BugCount = bugsCount,
                FeatureCount = featureCount,
            };
            return entry;
        }
    }
}
