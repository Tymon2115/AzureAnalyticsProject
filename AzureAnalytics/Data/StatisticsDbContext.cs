using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureAnalytics.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureAnalytics.Data
{
    public class StatisticsDbContext : DbContext
    {

        public DbSet<StatisticEntry> Statistics { get; set; }

        public StatisticsDbContext(DbContextOptions options) : base(options)
        {
        }


    }
}
