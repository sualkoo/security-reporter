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
        private DashboardService DashboardService { get; }

        public DashboardController(ILogger<ProjectReportController> logger, ILogger<ExceptionHandlingControllerBase> baseLogger, DashboardService dashboardService) : base(baseLogger)
        {
            DashboardService = dashboardService;
            Logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> getCriticalityData()
        {
            Logger.LogInformation("Received Get request for getting Criticality data for graph");
            return await HandleExceptionAsync(async () =>
            {
                int[] data = await DashboardService.GetCriticalityData();
                return Ok(data);
            });

        }
    }
}
