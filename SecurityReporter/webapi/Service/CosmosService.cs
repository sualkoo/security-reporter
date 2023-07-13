using Microsoft.Azure.Cosmos;
using webapi.Models;
using webapi.ProjectSearch.Models;

namespace webapi.Service
{
    public class CosmosService : ICosmosService
    {
        private string PrimaryKey { get; set; }
        private string EndpointUri { get; } = "https://localhost:8081";
        private string DatabaseName { get; } = "ProjectDatabase";
        private string ContainerName { get; } = "ProjectContainer";
        private Container Container { get; }

        public CosmosService(IConfiguration configuration)
        {
            PrimaryKey = configuration["DB:PrimaryKey"];
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
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

        public Task<bool> AddProjectReport(ProjectReportData data)
        {
            throw new NotImplementedException();
        }
    }
}
