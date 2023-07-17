
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
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
        private Microsoft.Azure.Cosmos.Container Container { get; }
        private Microsoft.Azure.Cosmos.Container ReportContainer { get; }

        public CosmosService(IConfiguration configuration)
        {
            PrimaryKey = configuration["DB:PrimaryKey"];
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
            ReportContainer = cosmosClient.GetContainer(DatabaseName, ReportContainerName);
        }

        public async Task<bool> AddProject(ProjectData data)
        {
            data.RequestCreated = DateTime.Now;
            if (data.Commments != null)
            {
                data.Commments[0].CreatedAt = DateTime.Now;
            }
            Console.WriteLine("Adding data to database.");
            try
            {
                await Container.CreateItemAsync(data);
                Console.WriteLine("Added to DB successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex);
                return false;
            }
        }

        public async Task<bool> AddProjectReport(ProjectReportData data)
        {
            try
            {
                data.Id = Guid.NewGuid();
                await ReportContainer.CreateItemAsync<ProjectReportData>(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ProjectReportData>> GetProjectReportDatasFiltered(int type, string value)
        {
            string query = "SELECT * FROM c WHERE";
            List<ProjectReportData> results = new List<ProjectReportData>();
            switch (type)
            {
                case 0:
                    query = query + " c.DocumentInfo.ProjectReportName = '" + value + "'";
                    break;
                    case 1:

                    break;
                default:
                    throw new Exception("Error aquiring Filter");
                    break;

            }
            try
            {
                QueryDefinition queryDefinition = new QueryDefinition(query);
                FeedIterator<ProjectReportData> queryResultSetIterator = ReportContainer.GetItemQueryIterator<ProjectReportData>(query);
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<ProjectReportData> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    results.AddRange(currentResultSet.ToList());
                }
                return results;
            }
            catch (Exception) 
            {
                throw new Exception("Error getting report data from Project Report Database");
            }
            return results;
        }
    }
}
