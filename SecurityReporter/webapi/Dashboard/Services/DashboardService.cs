using System.ComponentModel.DataAnnotations;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;
using webapi.Service;

namespace webapi.Dashboard.Services
{
    public class DashboardService : IDashboardService
    {
        public ICosmosService CosmosService { get; set; }
        private readonly ILogger Logger;

        public DashboardService(ICosmosService cosmosService)
        {
            CosmosService = cosmosService;
            ILoggerFactory loggerFactory = LoggerProvider.GetLoggerFactory();
            Logger = loggerFactory.CreateLogger<ProjectDataValidator>();
        }

        public async Task<List<Tuple<string, int>>> GetCriticalityData()
        {
            Logger.LogInformation($"Fetching Criticality data");

            return await CosmosService.GetCriticalityData();
        }

        public async Task<List<Tuple<string, int>>> GetVulnerabilityData()
        {
            Logger.LogInformation($"Fetching Vulnerability data");

            return await CosmosService.GetVulnerabilityData();
        }
    }
}
