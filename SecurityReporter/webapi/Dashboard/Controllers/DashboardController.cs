using Microsoft.AspNetCore.Mvc;
using webapi.Dashboard.Services;
using webapi.ProjectSearch.Controllers;

namespace webapi.Dashboard.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ExceptionHandlingControllerBase
{
    private new readonly ILogger Logger;

    public DashboardController(ILogger<DashboardController> logger, ILogger<ExceptionHandlingControllerBase> baseLogger,
        IDashboardService dashboardService) : base(baseLogger)
    {
        DashboardService = dashboardService;
        Logger = logger;
    }

    private IDashboardService DashboardService { get; }

    [HttpGet("Criticality")]
    public async Task<IActionResult> GetCriticalityData()
    {
        Logger.LogInformation("Received Get request for getting Criticality data for graph");
        return await HandleExceptionAsync(async () =>
        {
            var data = await DashboardService.GetCriticalityData();
            return Ok(data);
        });
    }

    [HttpGet("Vulnerability")]
    public async Task<IActionResult> GetVulnerabilityData()
    {
        Logger.LogInformation("Received Get request for getting Vulnerability data for graph");
        return await HandleExceptionAsync(async () =>
        {
            var data = await DashboardService.GetVulnerabilityData();
            return Ok(data);
        });
    }

    [HttpGet("CWE")]
    public async Task<IActionResult> GetCweData()
    {
        Logger.LogInformation("Received Get request for getting CWE data for graph");
        return await HandleExceptionAsync(async () =>
        {
            var data = await DashboardService.GetCweData();
            return Ok(data);
        });
    }

    [HttpGet("CVSS")]
    public async Task<IActionResult> GetCvssData()
    {
        Logger.LogInformation("Received Get request for getting CVSS data for graph");
        return await HandleExceptionAsync(async () =>
        {
            var data = await DashboardService.GetCvssData();
            return Ok(data);
        });
    }
}