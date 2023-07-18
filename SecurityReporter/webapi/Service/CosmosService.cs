using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using webapi.Models;
using webapi.ProjectSearch.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webapi.Service
{
    public class CosmosService : ICosmosService
    {
        private string PrimaryKey { get; set; }
        private string EndpointUri { get; } = "https://localhost:8081";
        private string DatabaseName { get; } = "ProjectDatabase";
        private string ContainerName { get; } = "ProjectContainer";
        private Microsoft.Azure.Cosmos.Container Container { get; }

        public CosmosService(IConfiguration configuration)
        {
            PrimaryKey = configuration["DB:PrimaryKey"];
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
        }

        public async Task<bool> AddProject(ProjectData data)
        {
            data.RequestCreated = DateTime.Now;
            if (data.Comments != null)
            {
                data.Comments[0].CreatedAt = DateTime.Now;
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

        public Task<bool> AddProjectReport(ProjectReportData data)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetNumberOfProjects()
        {
            QueryDefinition query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c");
            FeedIterator<int> queryResultIterator = Container.GetItemQueryIterator<int>(query);

            if (queryResultIterator.HasMoreResults)
            {
                FeedResponse<int> response = await queryResultIterator.ReadNextAsync();
                int count = response.FirstOrDefault();
                return count;
            }
            else
            {
                return -1;
            }
        }

        public async Task<List<ProjectData>> GetItems(int pageSize, int pageNumber)
        {
            int skipCount = pageSize * (pageNumber - 1);
            int itemCount = pageSize;

            QueryDefinition query = new QueryDefinition("SELECT * FROM c OFFSET @skipCount LIMIT @itemCount")
                .WithParameter("@skipCount", skipCount)
                .WithParameter("@itemCount", itemCount);

            List<ProjectData> items = new List<ProjectData>();
            FeedIterator<ProjectData> resultSetIterator = Container.GetItemQueryIterator<ProjectData>(query);

            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<ProjectData> response = await resultSetIterator.ReadNextAsync();
                items.AddRange(response.Resource);
            }
            return items;
        }
    }
}
