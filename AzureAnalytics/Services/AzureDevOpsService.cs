using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzureAnalytics.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureAnalytics.Services
{
    public class AzureDevOpsService
    {
        private readonly HttpClient _httpClient;
        private readonly AzureDevOpsConfig _config;
        private readonly string _project;


        public AzureDevOpsService(HttpClient httpClient, IOptions<AzureDevOpsConfig> config)
        {
            _httpClient = httpClient;
            _config = config.Value;
            _project = config.Value.Project;

            if (string.IsNullOrWhiteSpace(_config.Organization) ||
                string.IsNullOrWhiteSpace(_config.PersonalAccessToken))
            {
                throw new InvalidOperationException("Azure DevOps configuration is missing.");
            }

            _httpClient.BaseAddress = new Uri($"https://dev.azure.com/{_config.Organization}/");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($":{_config.PersonalAccessToken}")));

        }

        public async Task<WorkItemBatchResponse> GetWorkItemStatisticsAsync()
        {
            var query = new
            {
                query = "SELECT [System.Id], [System.WorkItemType] FROM WorkItems WHERE [System.WorkItemType] IN ('Feature', 'Bug')"
            };

            var response = await _httpClient.PostAsync(
                $"{_project}/_apis/wit/wiql?api-version=7.2-preview",
                new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Content: {errorContent}");
                throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var ids = string.Join(",", json["workItems"].Select(wi => wi["id"]).ToList());

            var fields = "System.WorkItemType";
            var workItemsResponse = await _httpClient.GetAsync(
                $"_apis/wit/workitems?ids={ids}&fields={fields}&api-version=7.2-preview");

            workItemsResponse.EnsureSuccessStatusCode();

            var workItemsContent = await workItemsResponse.Content.ReadAsStringAsync();
            var workItemBatchResponse = JsonConvert.DeserializeObject<WorkItemBatchResponse>(workItemsContent);

            return workItemBatchResponse;
        }

    }
}
