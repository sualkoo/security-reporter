using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Container = Microsoft.Azure.Cosmos.Container;

namespace cosmosTools.Service
{
    public class CosmosToolsService : ICosmosToolsService
    {
        private string PrimaryKey { get; set; }
        private string EndpointUri { get; } = "https://localhost:8081";
        private string DatabaseName { get; } = "ProjectDatabase";
        private string ContainerName { get; } = "ProjectContainer";
        private Container Container { get; }

        public CosmosToolsService(IConfiguration configuration)
        {
            PrimaryKey = configuration["DB:PrimaryKey"];
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
        }
        public Task<bool> AddProjects(int amount)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAllProjects()
        {
            Console.WriteLine("Deleting all data from database.");
            var query = new QueryDefinition("SELECT * FROM c");
            // Iterate over the query results and delete each item
            using (FeedIterator<dynamic> resultSetIterator = Container.GetItemQueryIterator<dynamic>(query))
            {
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<dynamic> response = await resultSetIterator.ReadNextAsync();
                    foreach (dynamic item in response)
                    {
                        string itemId = item.id;
                        // Delete the item from the container
                        ItemResponse<dynamic> deleteResponse = await Container.DeleteItemAsync<dynamic>(itemId, new PartitionKey(itemId));
                        Console.WriteLine($"{itemId},Deleted from DB successfully.");
                    }
                }
            }
            Console.WriteLine("All items in the container have been deleted.");
            return true;
        }
    }
}
