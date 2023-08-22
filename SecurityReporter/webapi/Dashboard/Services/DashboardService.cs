using webapi.ProjectSearch.Services;
using webapi.Service;

namespace webapi.Dashboard.Services;

public class DashboardService : IDashboardService
{
    private readonly ILogger logger;

    public DashboardService(ICosmosService cosmosService)
    {
        CosmosService = cosmosService;
        var loggerFactory = LoggerProvider.GetLoggerFactory();
        logger = loggerFactory.CreateLogger<ProjectDataValidator>();
    }

    private ICosmosService CosmosService { get; set; }

    public async Task<List<Tuple<string, int, int>>> GetCriticalityData()
    {
        logger.LogInformation("Fetching Criticality data");

        return await CosmosService.GetCriticalityData();
    }

    public async Task<List<Tuple<string, int, int>>> GetVulnerabilityData()
    {
        logger.LogInformation("Fetching Vulnerability data");

        return await CosmosService.GetVulnerabilityData();
    }

    public async Task<List<Tuple<int, int>>> GetCweData()
    {
        logger.LogInformation("Fetching CWE data");

        return await CosmosService.GetCWEData();
    }

    public async Task<List<Tuple<float, string, string>>> GetCvssData()
    {
        logger.LogInformation("Fetching CVSS data");

        return await CosmosService.GetCVSSData();
    }
}