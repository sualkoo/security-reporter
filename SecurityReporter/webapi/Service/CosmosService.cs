using Microsoft.Azure.Cosmos;
using System.Net;
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

        public Task<bool> AddProjectReport(ProjectReportData data)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> DeleteProject(string projectId)
        {
            Console.WriteLine("Deleting data from database.");
            try
            {
                ItemResponse<ProjectData> response = await Container.DeleteItemAsync<ProjectData>(projectId, new PartitionKey(projectId));

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    Console.WriteLine("Deleted from DB successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to delete from DB.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex);
                return false;
            }
        }


        public async Task<List<string>> DeleteProjects(List<string> projectIds)
        {
            Console.WriteLine("Deleting data from database.");

            var failed_to_delete = new List<string>();


            foreach (var id in projectIds)
            {
                try
                {
                    ItemResponse<ProjectData> response = await Container.DeleteItemAsync<ProjectData>(id, new PartitionKey(id));


                    Console.WriteLine($"{id},Deleted from DB successfully.");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{id}, Failed to delete from DB.");
                    failed_to_delete.Add(id);
                }
            }

            return failed_to_delete;
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
