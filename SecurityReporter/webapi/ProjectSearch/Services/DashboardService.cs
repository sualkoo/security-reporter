using System.ComponentModel.DataAnnotations;
using webapi.ProjectSearch.Models;
using webapi.Service;

namespace webapi.ProjectSearch.Services
{
    public class DashboardService
    {
        public ICosmosService CosmosService { get; set; }
        private readonly ILogger Logger;

        public DashboardService(ICosmosService cosmosService)
        {
            CosmosService = cosmosService;
            ILoggerFactory loggerFactory = LoggerProvider.GetLoggerFactory();
            Logger = loggerFactory.CreateLogger<ProjectDataValidator>();
        }

        public async Task<int[]> GetCriticalityData()
        {
            Logger.LogInformation($"Fetching Criticality data");

            return await CosmosService.GetCriticalityData();
        }
    }
}
