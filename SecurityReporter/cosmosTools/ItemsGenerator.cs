
using Microsoft.Azure.Cosmos;
using System.Configuration;
using webapi.Service;

namespace cosmosTools
{
    internal class ItemsGenerator
    {
        private string CosmosEndpoint { get; set; }
        private string CosmosKey { get; set; }
        private string DatabaseId { get; set; }
        private string ContainerId { get; set; }
        public ItemsGenerator(string primaryKey)
        {
            this.CosmosKey = primaryKey;
            this.DatabaseId = "ProjectDatabase";
            this.ContainerId = "ProjectContainer";
            this.CosmosEndpoint = "https://localhost:8081";
        }

        public void Help()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine("add [number] --> add [number] of random items to database");
            Console.WriteLine("clear        --> deletes all items from database");
            Console.WriteLine("quit         --> exit program");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        public async Task AddItemsToDatabaseAsync(int amount)
        {
            
            using (CosmosClient cosmosClient = new CosmosClient(CosmosEndpoint, CosmosKey))
            {
                Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
                Container container = await database.CreateContainerIfNotExistsAsync(ContainerId, "/id");
                
                

            }


            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("Added " + amount + " items into database.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public async Task ClearDatabaseAsync()
        {
            
            using (CosmosClient cosmosClient = new CosmosClient(CosmosEndpoint, CosmosKey))
            {
                Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
                Container container = await database.CreateContainerIfNotExistsAsync(ContainerId, "/id");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
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
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
        }
    }
}
