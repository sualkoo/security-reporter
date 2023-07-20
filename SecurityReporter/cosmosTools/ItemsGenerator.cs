
using Microsoft.Azure.Cosmos;
using System.Globalization;
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
        private CosmosService cosmosService { get; }
        public ItemsGenerator(string primaryKey)
        {
            CosmosKey = primaryKey;
            DatabaseId = "ProjectDatabase";
            ContainerId = "ProjectContainer";
            CosmosEndpoint = "https://localhost:8081";
            cosmosService = new CosmosService(CosmosKey, DatabaseId, ContainerId, CosmosEndpoint);

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

        public Random random = new Random();

        public async Task GenerateRandomData(int amount)
        {
            // Generate random data and add it to the list
            for (int i = 0; i < amount; i++)
            {
                ProjectData data = new ProjectData
                {
                    id = Guid.NewGuid(),
                    ProjectName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(GenerateRandomString(10)),
                    ProjectStatus = GetRandomElement(Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>().ToArray()),
                    ProjectQuestionare = GetRandomElement(Enum.GetValues(typeof(ProjectQuestionare)).Cast<ProjectQuestionare>().ToArray()),
                    ProjectScope = GetRandomElement(Enum.GetValues(typeof(ProjectScope)).Cast<ProjectScope>().ToArray()),
                    PentestDuration = random.Next(1, 1000),
                    CatsNumber = GenerateRandomString(10),
                    ProjectOfferStatus = GetRandomElement(Enum.GetValues(typeof(ProjectOfferStatus)).Cast<ProjectOfferStatus>().ToArray()),
                    PentestAspects = GenerateRandomString(15),
                    WorkingTeam = new List<string> { GenerateRandomString(12) },
                    ProjectLead = GenerateRandomString(10),
                    ReportStatus = GenerateRandomString(8),
                    ContactForClients = new List<string> { GenerateRandomEmail() },
                };

                DateTime startDate = GenerateRandomDateData();
                data.StartDate = DateOnly.FromDateTime(startDate);
                DateTime endDate = GenerateRandomDateData(startDate, startDate.AddMonths(random.Next(1, 12)));
                data.EndDate = DateOnly.FromDateTime(endDate);
                DateTime iko = GenerateRandomDateData(startDate, endDate);
                data.IKO = DateOnly.FromDateTime(iko);
                DateTime tko = GenerateRandomDateData(startDate, endDate);
                data.TKO = DateOnly.FromDateTime(tko);


                var comment = new Comment();
                comment.Text = GenerateRandomString(random.Next(1, 50));
                var Comments = new List<Comment> { };
                Comments.Add(comment);
                data.Comments = Comments;




                DateTime reportDueDate = GenerateRandomDateData(endDate, endDate.AddMonths(random.Next(1, 6)));
                data.ReportDueDate = DateOnly.FromDateTime(reportDueDate);


                await cosmosService.AddProject(data);
            }
        }

        public string GenerateRandomEmail()
        {
            return $"{GenerateRandomString(random.Next(5, 20))}@{GenerateRandomString(random.Next(5, 20))}.{GenerateRandomString(random.Next(2, 4))}";
        }

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public DateTime GenerateRandomDateData(DateTime startDate = default, DateTime endDate = default)
        {
            if (startDate == default)
                startDate = DateTime.Now.AddMonths(-12);

            if (endDate == default)
                endDate = DateTime.Now.AddMonths(12).Date;

            if (endDate <= startDate)
            {
                var temp = startDate;
                startDate = endDate;
                endDate = temp;
            }

            int range = (int)(endDate - startDate).TotalDays;
            var randomDate = startDate.AddDays(random.Next(Math.Max(1, range + 1)));

            return randomDate;
        }

        public T GetRandomElement<T>(T[] array)
        {
            return array[random.Next(array.Length)];
        }


    }
}

