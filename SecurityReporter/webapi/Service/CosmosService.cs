
using Azure;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using webapi.Models;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;

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
        private readonly ILogger Logger;

        public CosmosService(IConfiguration configuration)
        {
            PrimaryKey = configuration["DB:PrimaryKey"];
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
            ReportContainer = cosmosClient.GetContainer(DatabaseName, ReportContainerName);
            ILoggerFactory loggerFactory = LoggerProvider.GetLoggerFactory();
            Logger = loggerFactory.CreateLogger<ProjectDataValidator>();
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
            Logger.LogInformation("Adding project report to database.");
            try
            {
                data.Id = Guid.NewGuid();
                await ReportContainer.CreateItemAsync<ProjectReportData>(data);
                Logger.LogInformation("Project Report was successfuly saved to DB");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occured while saving new Project Report to DB: " + ex);
                throw new CustomException(StatusCodes.Status500InternalServerError, "An error occured while saving new Project Report to DB");
            }
        }

        public async Task<ProjectReportData> GetProjectReport(string projectId)
        {
            Microsoft.Azure.Cosmos.PartitionKey partitionKey = new Microsoft.Azure.Cosmos.PartitionKey(projectId);
            try
            {
                Logger.LogInformation("Searching for Report based on Id");
                ProjectReportData data = await ReportContainer.ReadItemAsync<ProjectReportData>(projectId, partitionKey);
                return data;
            }
            catch (Microsoft.Azure.Cosmos.CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Logger.LogWarning("Report with searched ID not found");
                throw new CustomException(StatusCodes.Status404NotFound, "Report with searched ID not found");
            }
            catch (Exception ex)
            {
                Logger.LogError("Unexpected error occurred during report fetching by ID: " + ex);
                throw new CustomException(StatusCodes.Status500InternalServerError, "Unexpected error occurred");
            }
        }
        public async Task<List<ProjectReportData>> GetProjectReports(string? subcategory, string keyword, string value)
        {
            List<ProjectReportData> results = new List<ProjectReportData>();
            string query = "SELECT * FROM c WHERE ";

            if (!string.IsNullOrEmpty(subcategory))
            {
                query = $"{query} LOWER(c[@subcategory][@keyword]) LIKE LOWER(@value)";
            }
            else
            {
                query = $"{query} LOWER(c[@keyword]) LIKE LOWER(@value)";
            }
            try
            {
                Logger.LogInformation("Fetching reports from the database");
                QueryDefinition queryDefinition = new QueryDefinition(query).WithParameter("@subcategory", $"{subcategory}")
                                                                            .WithParameter("@value", $"%{value}%")
                                                                            .WithParameter("@keyword", $"{keyword}");
                FeedIterator<ProjectReportData> queryResultSetIterator = ReportContainer.GetItemQueryIterator<ProjectReportData>(queryDefinition);
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<ProjectReportData> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    results.AddRange(currentResultSet.ToList());
                }
                Logger.LogInformation("Returning found reports");
                return results;
            }
            catch (Exception exception)
            {
                Logger.LogError("Unexpected error occurred during report fetching by keywords: " + exception);
                throw new CustomException(StatusCodes.Status500InternalServerError, "Unexpected error occurred");
            }
        }

        public async Task<PagedDBResults<List<ProjectReportData>>> GetPagedProjectReports(string? subcategory, string keyword, string value, int page)
        {
            int limit = 24; 
            int offset = limit * (page - 1);
            int totalResults;
            List<ProjectReportData> data = new List<ProjectReportData>();

            

            string query = "SELECT * FROM c WHERE ";
            string queryCount = "SELECT VALUE COUNT(1) FROM c WHERE";

            if (!string.IsNullOrEmpty(subcategory))
            {
                query = $"{query} LOWER(c[@subcategory][@keyword]) LIKE LOWER(@value) OFFSET @offset LIMIT @limit";
                queryCount = $"{queryCount} LOWER(c[@subcategory][@keyword]) LIKE LOWER(@value)";
            }
            else
            {
                query = $"{query} LOWER(c[@keyword]) LIKE LOWER(@value) OFFSET @offset LIMIT @limit";
                queryCount = $"{queryCount} LOWER(c[@keyword]) LIKE LOWER(@value)";
            }

            try
            {
                Logger.LogInformation("Fetching reports from the database");
                QueryDefinition queryDefinition = new QueryDefinition(query).WithParameter("@subcategory", $"{subcategory}")
                                                                            .WithParameter("@value", $"%{value}%")
                                                                            .WithParameter("@keyword", $"{keyword}")
                                                                            .WithParameter("@offset", offset)
                                                                            .WithParameter("@limit", limit);
                FeedIterator<ProjectReportData> queryResultSetIterator = ReportContainer.GetItemQueryIterator<ProjectReportData>(queryDefinition);
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<ProjectReportData> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    data.AddRange(currentResultSet.ToList());
                }
                Logger.LogInformation("Returning found reports");


                QueryDefinition queryDefinitioCount = new QueryDefinition(queryCount).WithParameter("@subcategory", $"{subcategory}")
                                                                                     .WithParameter("@value", $"%{value}%")
                                                                                     .WithParameter("@keyword", $"{keyword}");

                FeedIterator<int> resultSetIterator = ReportContainer.GetItemQueryIterator<int>(queryDefinitioCount);
                FeedResponse<int> response = await resultSetIterator.ReadNextAsync();
                totalResults = response.FirstOrDefault();

                PagedDBResults<List<ProjectReportData>> results = new PagedDBResults<List<ProjectReportData>>(data, page);
                results.TotalRecords = totalResults;
                results.TotalPages = (int)Math.Ceiling((double)totalResults / limit);

                UriBuilder uriBuilder = new UriBuilder("https://localhost:7075/project-reports");
                string queryPage = uriBuilder.Query;
                if (results.TotalPages > page)
                {
                    if (!string.IsNullOrEmpty(subcategory))
                    {
                        queryPage += "subcategory=" + Uri.EscapeDataString(subcategory);
                    }
                    queryPage += "&keyword=" + Uri.EscapeDataString(keyword) + "&value=" + Uri.EscapeDataString(value) + "&page=" + (page + 1);

                    uriBuilder.Query = queryPage.TrimStart('?');
                    results.NextPage = uriBuilder.Uri;
                }

                return results;
            }
            catch (Exception exception)
            {
                Logger.LogError("Unexpected error occurred during report fetching by keywords: " + exception);
                throw new CustomException(StatusCodes.Status500InternalServerError, "Unexpected error occurred");
            }
        }
    }
}
