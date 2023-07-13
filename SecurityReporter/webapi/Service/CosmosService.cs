
using Microsoft.Azure.Cosmos;
using webapi.Models;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Models;

namespace webapi.Service
{
    public class CosmosService : ICosmosService
    {
        private string PrimaryKey { get; set; }
        private string EndpointUri { get; } = "https://localhost:8081";
        private string DatabaseName { get; } = "ProjectDatabase";
        private string ContainerName { get; } = "ProjectContainer";
        private string ReportContainerName { get; } = "ProjectReportContainer";
        private Container Container { get; }
        private Container ReportContainer { get; }

        public CosmosService(IConfiguration configuration)
        {
            PrimaryKey = configuration["DB:PrimaryKey"];
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
            ReportContainer = cosmosClient.GetContainer(DatabaseName, ReportContainerName);
        }

        public async Task<bool> AddProject(ProjectData data)
        {
            try
            {
                await Container.CreateItemAsync(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddProjectReport(ProjectReportData data)
        {
            try
            {
                await ReportContainer.CreateItemAsync<ProjectReportData>(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
