using Newtonsoft.Json;

namespace AzureAnalytics.Models
{
    public class WorkItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fields")]
        public WorkItemFields Fields { get; set; }

        public string WorkItemType => Fields?.Type;
        public string Title => Fields?.Title;
    }

}
