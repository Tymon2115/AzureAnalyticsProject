using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureAnalytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureAnalytics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            try
            {

                var statistics = await _statisticsService.GetStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
