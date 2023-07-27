using Microsoft.Azure.Cosmos;
using webapi.Enums;
using webapi.Models;
using webapi.Service;


namespace cosmosTools
{
    internal class ItemsGenerator
    {
        private string CosmosEndpoint { get; set; }
        private string CosmosKey { get; set; }
        private string DatabaseId { get; set; }
        private string ContainerId { get; set; }
        private CosmosService CosmosService { get; }


        public ItemsGenerator(string primaryKey)
        {
            CosmosKey = primaryKey;
            DatabaseId = "ProjectDatabase";
            ContainerId = "ProjectContainer";
            CosmosEndpoint = "https://localhost:8081";
            CosmosService = new CosmosService(CosmosKey, DatabaseId, ContainerId, CosmosEndpoint);

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

            await GenerateRandomData(amount);

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


        public async Task GenerateRandomData(int amount)
        {
            // Generate random data and add it to the list

            var Generators = new Generators();
            var randomWords = await Generators.GenerateWords();

            for (int i = 0; i < amount; i++)
            {
                ProjectData data = new ProjectData
                {
                    id = Guid.NewGuid(),
                    ProjectName = Generators.GenerateProjectName(),
                    ProjectStatus = Generators.GenerateRandomElement(Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>().ToArray()),
                    ProjectQuestionare = Generators.GenerateRandomElement(Enum.GetValues(typeof(ProjectQuestionare)).Cast<ProjectQuestionare>().ToArray()),
                    ProjectScope = Generators.GenerateRandomElement(Enum.GetValues(typeof(ProjectScope)).Cast<ProjectScope>().ToArray()),
                    PentestDuration = new Random().Next(2, 11),
                    CatsNumber = Generators.GenerateRandomString(10),
                    ProjectOfferStatus = Generators.GenerateRandomElement(Enum.GetValues(typeof(ProjectOfferStatus)).Cast<ProjectOfferStatus>().ToArray()),
                    PentestAspects = Generators.GeneratePentest(),
                    ReportStatus = Generators.GenerateReportStatus(),
                    ContactForClients = Generators.GenerateContacts(),
                    WorkingTeam = Generators.GenerateWorkingTeam(),
                    Comments = Generators.GenerateComment(randomWords),
                };

                data.ProjectLead = data.WorkingTeam[new Random().Next(0, data.WorkingTeam.Count())];

                DateTime startDate = Generators.GenerateRandomDateData();
                data.StartDate = DateOnly.FromDateTime(startDate);
                DateTime endDate = Generators.GenerateRandomDateData(startDate, startDate.AddMonths(new Random().Next(1, 12)));
                data.EndDate = DateOnly.FromDateTime(endDate);
                DateTime iko = Generators.GenerateRandomDateData(startDate, endDate);
                data.IKO = DateOnly.FromDateTime(iko);
                DateTime tko = Generators.GenerateRandomDateData(startDate, endDate);
                data.TKO = DateOnly.FromDateTime(tko);

                DateTime reportDueDate = Generators.GenerateRandomDateData(endDate, endDate.AddMonths(new Random().Next(1, 6)));
                data.ReportDueDate = DateOnly.FromDateTime(reportDueDate);


                await CosmosService.AddProject(data);
            }
        }
    }
}

