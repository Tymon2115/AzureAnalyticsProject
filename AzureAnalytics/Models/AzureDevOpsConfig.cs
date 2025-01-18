using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAnalytics.Models
{
    public class AzureDevOpsConfig
    {
        public string Organization { get; set; }
        public string PersonalAccessToken { get; set; }
        public string Project { get; set; }
    }

}
