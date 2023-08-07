using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("dashboard")]
    public class DashboardController : ExceptionHandlingControllerBase
    {
        private readonly ILogger Logger;
        private IDashboardService DashboardService { get; }

        public DashboardController(ILogger<DashboardController> logger, ILogger<ExceptionHandlingControllerBase> baseLogger, IDashboardService dashboardService) : base(baseLogger)
        {
            DashboardService = dashboardService;
            Logger = logger;
        }

        [HttpGet("Criticality")]
        public async Task<IActionResult> getCriticalityData()
        {
            Logger.LogInformation("Received Get request for getting Criticality data for graph");
            return await HandleExceptionAsync(async () =>
            {
                List<Tuple<int, int>> data = await DashboardService.GetCriticalityData();
                return Ok(data);
            });

        }

        [HttpGet("Vulnerability")]
        public async Task<IActionResult> getVulnerabilityData()
        {
            Logger.LogInformation("Received Get request for getting Vulnerability data for graph");
            return await HandleExceptionAsync(async () =>
            {
                List<Tuple<int, int>> data = await DashboardService.GetVulnerabilityData();
                return Ok(data);
            });

        }
    }
}
