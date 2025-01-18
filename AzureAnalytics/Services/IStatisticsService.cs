using AzureAnalytics.Models;

namespace AzureAnalytics.Services
{
    public interface IStatisticsService
    {
        Task<IEnumerable<StatisticEntry>> GetStatisticsAsync();

        Task AddAsync(WorkItemBatchResponse entry);
    }
}
