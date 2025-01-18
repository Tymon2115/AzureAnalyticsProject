using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AzureAnalytics.Models
{
    public class WorkItemBatchResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("value")]
        public List<WorkItem> WorkItems { get; set; }
    }

}
