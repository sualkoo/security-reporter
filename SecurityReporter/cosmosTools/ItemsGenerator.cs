using Microsoft.Azure.Cosmos;

namespace cosmosTools
{
    internal class ItemsGenerator
    {       
        public ItemsGenerator()
        {
            
        }           

        public void Help()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("add [number] --> add [number] of random items to database");
            Console.WriteLine("clear --> deletes all items from database");
            Console.WriteLine();
        }        

        public async Task AddItemsToDatabaseAsync(int amount)
        {
            Console.WriteLine("Added " + amount + " items.");
            Console.WriteLine();
        }

        public async Task ClearDatabaseAsync()
        {
            string cosmosEndpoint = "https://localhost:8081";
            string cosmosKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            string databaseId = "ProjectDatabase";
            string containerId = "ProjectContainer";
            using (CosmosClient cosmosClient = new CosmosClient(cosmosEndpoint, cosmosKey))
            {
                Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
                Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/id");

                Console.WriteLine("Deleting all data from database.");
                var query = new QueryDefinition("SELECT * FROM c");

                // Iterate over the query results and delete each item
                using (FeedIterator<dynamic> resultSetIterator = container.GetItemQueryIterator<dynamic>(query))
                {
                    while (resultSetIterator.HasMoreResults)
                    {
                        FeedResponse<dynamic> response = await resultSetIterator.ReadNextAsync();
                        foreach (dynamic item in response)
                        {
                            string itemId = item.id;
                            // Delete the item from the container
                            ItemResponse<dynamic> deleteResponse = await container.DeleteItemAsync<dynamic>(itemId, new PartitionKey(itemId));
                            Console.WriteLine($"{itemId},Deleted from DB successfully.");
                        }
                    }
                }
                Console.WriteLine("All items in the container have been deleted.");
            }
        }
    }
}
