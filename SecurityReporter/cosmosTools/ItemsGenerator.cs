
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

namespace RandomDataGenerator
{
    public class ProjectData
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public ProjectQuestionare PublicQuestionare { get; set; }
        public ProjectScope ProjectScope { get; set; }
        public int PentestDuration { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string IKO { get; set; }
        public string TKO { get; set; }
        public string ReportDueDate { get; set; }
        public string RequestCreated { get; set; }
        public List<string> Commments { get; set; }
        public string CatsNumber { get; set; }
        public ProjectOfferStatus ProjectOfferStatus { get; set; }
        public string PentestAspects { get; set; }
        public List<string> WorkingTeam { get; set; }
        public string ProjectLead { get; set; }
        public string ReportStatus { get; set; }
        public List<string> ContactForClients { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
        public string _rid { get; set; }
        public string _self { get; set; }
        public string _etag { get; set; }
        public string _attachments { get; set; }
        public long _ts { get; set; }
    }

    public enum ProjectStatus
    {
        InProgress = 1, Completed = 2, OnHold = 3, SomeOtherStatus1 = 4, SomeOtherStatus2 = 5, SomeOtherStatus3 = 6
    }

    public enum ProjectQuestionare
    {
        Option1 = 1, Option2 = 2, Option3 = 3
    }

    public enum ProjectScope
    {
        Scope1 = 1, Scope2 = 2, Scope3 = 3
    }

    public enum ProjectOfferStatus
    {
        Status1 = 1, Status2 = 2, Status3 = 3, Status4 = 4, Status5 = 5, Status6 = 6, Status7 = 7, Status8 = 8, Status9 = 9, Status10 = 10
    }

    public static class DataGenerator
    {
        static Random random = new Random();

        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static int GenerateRandomNumberExceptZero(int minValue, int maxValue)
        {
            int number;
            do
            {
                number = random.Next(minValue, maxValue + 1);
            } while (number == 0);
            return number;
        }

        static DateTime GenerateRandomDateData(DateTime startDate = default, DateTime endDate = default)
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

        static T GetRandomElement<T>(T[] array)
        {
            return array[random.Next(array.Length)];
        }

        public static List<ProjectData> GenerateRandomData(int amount)
        {
            List<ProjectData> randomDataList = new List<ProjectData>();

            // Generate random data and add it to the list
            for (int i = 0; i < amount; i++)
            {
                ProjectData data = new ProjectData
                {
                    Id = Guid.NewGuid(),
                    ProjectName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(GenerateRandomString(10)),
                    ProjectStatus = GetRandomElement(Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>().ToArray()),
                    PublicQuestionare = GetRandomElement(Enum.GetValues(typeof(ProjectQuestionare)).Cast<ProjectQuestionare>().ToArray()),
                    ProjectScope = GetRandomElement(Enum.GetValues(typeof(ProjectScope)).Cast<ProjectScope>().ToArray()),
                    PentestDuration = GenerateRandomNumberExceptZero(1, 30),
                    Commments = new List<string> { GenerateRandomString(50) },
                    CatsNumber = GenerateRandomString(10),
                    ProjectOfferStatus = GetRandomElement(Enum.GetValues(typeof(ProjectOfferStatus)).Cast<ProjectOfferStatus>().ToArray()),
                    PentestAspects = GenerateRandomString(15),
                    WorkingTeam = new List<string> { GenerateRandomString(12) },
                    ProjectLead = GenerateRandomString(10),
                    ReportStatus = GenerateRandomString(8),
                    ContactForClients = new List<string> { GenerateRandomEmail() },
                    _rid = "eUB7AMBwAAkCAAAAAAAAAA==",  //question, what is this for?
                    _self = "dbs/eUB7AA==/colls/eUB7AMBwAAk=/docs/eUB7AMBwAAkCAAAAAAAAAA==/",//
                    _etag = "\"00000000-0000-0000-b8a9-7c5fc26b01d9\"",//
                    _attachments = "attachments/",
                    _ts = 1689596605
                };

                DateTime startDate = GenerateRandomDateData();
                DateTime endDate = GenerateRandomDateData(startDate, startDate.AddMonths(random.Next(1, 12)));
                DateTime iko = GenerateRandomDateData(startDate, endDate);
                DateTime tko = GenerateRandomDateData(startDate, endDate);

                data.StartDate = startDate.ToString("yyyy-MM-dd");
                data.EndDate = endDate.ToString("yyyy-MM-dd");
                data.IKO = iko.ToString("yyyy-MM-dd");
                data.TKO = tko.ToString("yyyy-MM-dd");

                if (endDate < startDate || endDate < tko || endDate < iko)
                {
                    var errorMessages = new List<string>();
                    if (endDate < startDate)
                    {
                        errorMessages.Add("End Date must be greater than or equal to StartDate.");
                    }
                    if (endDate < tko)
                    {
                        errorMessages.Add("End Date must be greater than or equal to TKO.");
                    }
                    if (endDate < iko)
                    {
                        errorMessages.Add("End Date must be greater than or equal to IKO.");
                    }
                    data.Errors["EndDate"] = errorMessages;
                }

                DateTime reportDueDate = GenerateRandomDateData(endDate, endDate.AddMonths(random.Next(1, 6)));
                data.ReportDueDate = reportDueDate.ToString("yyyy-MM-dd");
                if (reportDueDate < endDate)
                {
                    var errorMessages = new List<string>
                    {
                        "Report Due Date must be greater than or equal to End Date."
                    };
                    data.Errors["ReportDueDate"] = errorMessages;
                }

                randomDataList.Add(data);
            }

            return randomDataList;
        }

        static string GenerateRandomEmail()
        {
            string randomString = GenerateRandomString(10);
            return $"{randomString}@random.com";
        }
    }
}