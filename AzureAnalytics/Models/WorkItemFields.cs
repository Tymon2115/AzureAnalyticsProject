using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AzureAnalytics.Models
{
    public class WorkItemFields
    {
        [JsonProperty("System.WorkItemType")]
        public string Type { get; set; }

        [JsonProperty("System.Title")]
        public string Title { get; set; }
    }

}
