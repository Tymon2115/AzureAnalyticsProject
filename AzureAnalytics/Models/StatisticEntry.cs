using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAnalytics.Models
{
    public class StatisticEntry
    {
        public int Id { get; set; }
        public int BugCount { get; set; }

        public int FeatureCount { get; set; }

        public DateTime Date { get; set; }
    }
}
