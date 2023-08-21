using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Net;
using System.Text;
using webapi.Login.Services;
using webapi.Models;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.AspNetCore.Http;

namespace webapi.Service
{
    public class CosmosService : ICosmosService
    {
        private string PrimaryKey { get; set; }
        private string EndpointUri { get; } = "https://localhost:8081";
        private string DatabaseName { get; } = "ProjectDatabase";
        private string ContainerName { get; } = "ProjectContainer";

        private ClientMailService clientMailService;

        private RoleService roleService;

        private readonly IHttpContextAccessor httpContextAccessor;

        private string ReportContainerName { get; } = "ProjectReportContainer";
        private Microsoft.Azure.Cosmos.Container Container { get; }
        private Microsoft.Azure.Cosmos.Container ReportContainer { get; }
        private readonly ILogger Logger;

        public CosmosService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ClientMailService clientMailService, RoleService roleService)
        {
            PrimaryKey = configuration["DB:PrimaryKey"];
            if (string.IsNullOrEmpty(PrimaryKey))
            {
                EndpointUri = "https://security-reporter.documents.azure.com:443";
                PrimaryKey = "6sDm3pLgxLV7WnQqYkYPBmoyapf91CHvD1OpTJVBxNvYh6wRgmTEqJBy7kAR11MiTEEne6QV5G9dACDbdbjQSg==";
            }
            
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
            ReportContainer = cosmosClient.GetContainer(DatabaseName, ReportContainerName);
            ILoggerFactory loggerFactory = LoggerProvider.GetLoggerFactory();
            Logger = loggerFactory.CreateLogger<ProjectDataValidator>();
            this.clientMailService = clientMailService;
            this.roleService = roleService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public CosmosService(string primaryKey, string databaseId, string containerId, string cosmosEndpoint)
        {
            PrimaryKey = primaryKey;
            DatabaseName = databaseId;
            ContainerName = containerId;
            EndpointUri = cosmosEndpoint;
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

        public async Task<bool> DeleteProject(string id)
        {
            try
            {
                ItemResponse<ProjectData> response = await Container.DeleteItemAsync<ProjectData>(id, new PartitionKey(id));

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    Console.WriteLine($"{id}, Deleted from DB successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"{id}, Not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{id}, Not found.");
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
                    ItemResponse<ProjectData> response = await Container.ReadItemAsync<ProjectData>(id, new PartitionKey(id));
                    Console.WriteLine($"{id}, Found in DB successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{id}, Not found.");
                    failed_to_delete.Add($"{id}, Not found.");
                }
            }

            if (failed_to_delete.Count > 0)
            {
                Console.WriteLine("Deletion of Projects will not be completed.");
                return failed_to_delete;
            }

            foreach (var id in projectIds)
            {
                await DeleteProject(id);
            }
            Console.WriteLine("Deletion of Projects completed successfully.");

            return new List<string> { };
        }

        public async Task<int> GetNumberOfProjects()
        {

            bool client = false;
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                if (await roleService.GetUserRoleBySubjectId(httpContextAccessor.HttpContext.User?.FindFirst("sub")?.Value) == "client")
                {
                    client = true;
                }
            }

            var queryString = "SELECT VALUE COUNT(1) FROM c";
            if (client)
            {
                var mail = this.clientMailService.GetClientMail(httpContextAccessor.HttpContext.User?.FindFirst("sub")?.Value);
                queryString = $"SELECT VALUE COUNT(1) FROM c WHERE IS_DEFINED(c.ContactForClients) AND (ARRAY_CONTAINS(c.ContactForClients, \"{mail}\"))";
            }

            QueryDefinition query = new QueryDefinition(queryString);
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

        public async Task<List<ProjectList>> GetItems(int pageSize, int pageNumber, FilterData filter)
        {
            int skipCount = pageSize * (pageNumber - 1);
            int itemCount = pageSize;

            bool client = false;
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                if (await roleService.GetUserRoleBySubjectId(httpContextAccessor.HttpContext.User?.FindFirst("sub")?.Value) == "client") {
                    client = true;
                }
            }

            var queryString = "SELECT * FROM c";
            if (client) {
                var mail = this.clientMailService.GetClientMail(httpContextAccessor.HttpContext.User?.FindFirst("sub")?.Value);
                queryString = $"SELECT * FROM c WHERE IS_DEFINED(c.ContactForClients) AND (ARRAY_CONTAINS(c.ContactForClients, \"{mail}\"))";
            }

            var queryParameters = new Dictionary<string, object>();

            var filterConditions = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter.FilteredProjectName))
            {
                filterConditions.Add($"CONTAINS(LOWER(c.ProjectName), @projectName)");
                queryParameters["@projectName"] = filter.FilteredProjectName.ToLower();
            }

            if (filter.FilteredProjectStatus.HasValue)
            {
                filterConditions.Add("c.ProjectStatus = @projectStatus");
                queryParameters["@projectStatus"] = (int)filter.FilteredProjectStatus.Value;
            }

            if (filter.FilteredProjectQuestionare.HasValue)
            {
                filterConditions.Add("c.ProjectQuestionare = @questionnaire");
                queryParameters["@questionnaire"] = (int)filter.FilteredProjectQuestionare.Value;
            }

            if (filter.FilteredProjectScope.HasValue)
            {
                filterConditions.Add("c.ProjectScope = @projectScope");
                queryParameters["@projectScope"] = (int)filter.FilteredProjectScope.Value;
            }

            if (filter.FilteredStartDate.HasValue)
            {
                filterConditions.Add("c.StartDate >= @startDate");
                queryParameters["@startDate"] = filter.FilteredStartDate.Value.ToString("yyyy-MM-dd");
            }

            if (filter.FilteredEndDate.HasValue)
            {
                filterConditions.Add("c.EndDate <= @endDate");
                queryParameters["@endDate"] = filter.FilteredEndDate.Value.ToString("yyyy-MM-dd");
            }

            if (filter.FilteredPentestStart.HasValue && filter.FilteredPentestEnd.HasValue)
            {
                filterConditions.Add("c.PentestDuration >= @pentestDurationMin AND c.PentestDuration <= @pentestDurationMax");
                queryParameters["@pentestDurationMin"] = filter.FilteredPentestStart.Value;
                queryParameters["@pentestDurationMax"] = filter.FilteredPentestEnd.Value;
            }
            else if (filter.FilteredPentestStart.HasValue)
            {
                filterConditions.Add("c.PentestDuration >= @pentestDurationMin");
                queryParameters["@pentestDurationMin"] = filter.FilteredPentestStart.Value;
            }
            else if (filter.FilteredPentestEnd.HasValue)
            {
                filterConditions.Add("c.PentestDuration <= @pentestDurationMax");
                queryParameters["@pentestDurationMax"] = filter.FilteredPentestEnd.Value;
            }

            if (filter.FilteredIKO.HasValue)
            {
                if (filter.FilteredIKO.Value == 1)
                {
                    filterConditions.Add("IS_NULL(c.IKO)");
                }
                else if (filter.FilteredIKO.Value == 2)
                {
                    filterConditions.Add("NOT IS_NULL(c.IKO)");
                }
            }

            if (filter.FilteredTKO.HasValue)
            {
                if (filter.FilteredTKO.Value == 1)
                {
                    filterConditions.Add("IS_NULL(c.TKO)");
                }
                else if (filter.FilteredTKO.Value == 2)
                {
                    filterConditions.Add("NOT IS_NULL(c.TKO)");
                }
            }

            if (filterConditions.Count > 0)
            {
                if (client)
                {
                    queryString += " AND " + string.Join(" AND ", filterConditions);
                }
                else
                {
                    queryString += " WHERE " + string.Join(" AND ", filterConditions);
                }
            }

            queryString += " OFFSET @skipCount LIMIT @itemCount";
            queryParameters["@skipCount"] = skipCount;
            queryParameters["@itemCount"] = itemCount;

            var items = new List<ProjectList>();
            var queryDefinition = new QueryDefinition(queryString);
            foreach (var param in queryParameters)
            {
                queryDefinition.WithParameter(param.Key, param.Value);
            }
            var resultSetIterator = Container.GetItemQueryIterator<ProjectList>(queryDefinition);
            try
            {
                while (resultSetIterator.HasMoreResults)
                {
                    var response = await resultSetIterator.ReadNextAsync();
                    items.AddRange(response.Resource);
                    Console.WriteLine("Successfully fetched items from DB.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while fetching items from DB: " + ex);
                throw;
            }
            return items;
        
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
            PartitionKey partitionKey = new PartitionKey(projectId);
            try
            {
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

        public async Task<PagedDBResults<List<ProjectReportData>>> GetPagedProjectReports(string? subcategory, string keyword, string value, int page)
        {
            int limit = 24;
            if (page < 1)
            {
                page = 1;
            }
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

                UriBuilder uriBuilder = new UriBuilder("/project-reports");
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

        public async Task<PagedDBResults<List<FindingResponse>>> GetPagedProjectReportFindings(string? projectName, string? details, string? impact, string? repeatability, string? references, string? cWE, string value, int page)
        {
            int limit = 6;
            bool firstFilter = false;
            if (page < 1)
            {
                page = 1;
            }
            int offset = limit * (page - 1);
            int totalResults;
            int valueInt = 0;
            List<string> querypath = new List<string>();
            List<FindingResponse> newData = new List<FindingResponse>();

            //Building Queries

            string query = "SELECT DISTINCT VALUE {'ProjectReportId': c.id, 'ProjectReportName': c.DocumentInfo.ProjectReportName, 'Finding': f } " +
                            "FROM c " +
                            "JOIN f IN c.Findings " +
                            "JOIN r IN f.SubsectionReferences " +
                            "WHERE";

            string queryCount = "SELECT DISTINCT c.id, f.FindingName " +
                                "FROM c " +
                                "JOIN f IN c.Findings " +
                                "JOIN r IN f.SubsectionReferences " +
                                "WHERE";

            if (!string.IsNullOrEmpty(projectName))
            {
                querypath.Add(" LOWER(c.DocumentInfo.ProjectReportName) LIKE LOWER(@value) ");
            }
            if (!string.IsNullOrEmpty(details))
            {
                querypath.Add(" LOWER(f[@details]) LIKE LOWER(@value) ");
            }
            if (!string.IsNullOrEmpty(impact))
            {
                querypath.Add(" LOWER(f[@impact]) LIKE LOWER(@value) ");
            }
            if (!string.IsNullOrEmpty(repeatability))
            {
                querypath.Add(" LOWER(f[@repeatability]) LIKE LOWER(@value) ");
            }
            if (!string.IsNullOrEmpty(references))
            {
                querypath.Add(" LOWER(r) LIKE LOWER(@value) ");
            }
            if (!string.IsNullOrEmpty(cWE) && int.TryParse(value, out valueInt))
            {
                querypath.Add(" (f[@cwe]) = (@valueInt) ");
            }
            else if (!string.IsNullOrEmpty(cWE) && !int.TryParse(value, out valueInt))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Unable to convert string to int for CWE value");
            }
            if (querypath.Count() > 0)
            {
                foreach (var path in querypath)
                {
                    if (firstFilter)
                    {
                        query = $"{query} OR {path}";
                        queryCount = $"{queryCount} OR {path}";
                    }
                    else
                    {
                        query = $"{query} {path}";
                        queryCount = $"{queryCount} {path}";
                        firstFilter = true;
                    }
                }
            }
            else
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "At least one filter has to be selected");
            }
            query = $"{query}  ORDER BY c.DocumentInfo.ProjectReportName OFFSET @offset LIMIT @limit";
            queryCount = $" SELECT VALUE COUNT(1) FROM ( {queryCount} )";
            
            //Executing Queries
            //Reports
            Logger.LogInformation("Fetching reports from the database");
                QueryDefinition queryDefinition = new QueryDefinition(query).WithParameter("@value", $"%{value}%")
                                                                            .WithParameter("@offset", offset)
                                                                            .WithParameter("@valueInt", valueInt)
                                                                            .WithParameter("@details", $"{details}")
                                                                            .WithParameter("@impact", $"{impact}")
                                                                            .WithParameter("@repeatability", $"{repeatability}")
                                                                            .WithParameter("@cwe", $"{cWE}")
                                                                            .WithParameter("@limit", limit);

            FeedIterator<FindingResponse> queryResultSetIterator = ReportContainer.GetItemQueryIterator<FindingResponse>(queryDefinition);
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<FindingResponse> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                newData.AddRange(currentResultSet.ToList());
            }
            Logger.LogInformation("Returning found reports");

            //Total Results
            QueryDefinition queryDefinitionCount = new QueryDefinition(queryCount).WithParameter("@value", $"%{value}%")
                                                                                  .WithParameter("@details", $"{details}")
                                                                                  .WithParameter("@impact", $"{impact}")
                                                                                  .WithParameter("@repeatability", $"{repeatability}")
                                                                                  .WithParameter("@cwe", $"{cWE}")
                                                                                  .WithParameter("@valueInt", valueInt);

            FeedIterator<int> resultSetIterator = ReportContainer.GetItemQueryIterator<int>(queryDefinitionCount);
            FeedResponse<int> response = await resultSetIterator.ReadNextAsync();
            totalResults = response.FirstOrDefault();

            //Filling PagedDBResult

            PagedDBResults<List<FindingResponse>> results = new PagedDBResults<List<FindingResponse>>(newData, page);
            results.TotalRecords = totalResults;
            results.TotalPages = (int)Math.Ceiling((double)totalResults / limit);

            //Building URL for next page
            UriBuilder uriBuilder = new UriBuilder("/project-reports/findings");
            string queryPage = uriBuilder.Query;
            if (results.TotalPages > page)
            {
                if (!string.IsNullOrEmpty(projectName))
                {
                    queryPage += "ProjectName=" + Uri.EscapeDataString(projectName);
                }
                if (!string.IsNullOrEmpty(details))
                {
                    queryPage += "&Details=" + Uri.EscapeDataString(details);
                }
                if (!string.IsNullOrEmpty(impact))
                {
                    queryPage += "&Impact=" + Uri.EscapeDataString(impact);
                }
                if (!string.IsNullOrEmpty(repeatability))
                {
                    queryPage += "&Repeatability=" + Uri.EscapeDataString(repeatability);
                }
                if (!string.IsNullOrEmpty(references))
                {
                    queryPage += "&References=" + Uri.EscapeDataString(references);
                }
                if (!string.IsNullOrEmpty(cWE))
                {
                    queryPage += "&CWE=" + Uri.EscapeDataString(cWE);
                }
                queryPage += "&value=" + Uri.EscapeDataString(value) + "&page=" + (page + 1);

                uriBuilder.Query = queryPage.TrimStart('?');
                results.NextPage = uriBuilder.Uri;
            }

            return results;
        }

        public async Task<ProjectData> GetProjectById(string id)
        {
            try
            {
                ItemResponse<ProjectData> response = await Container.ReadItemAsync<ProjectData>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving project by ID: {ex.Message}", ex);
            }
        }

        public async Task<bool> GetUploadStatus(string id)
        {
            try
            {
                var project = await GetProjectById(id); // You should implement GetProjectById in your service

                if (project != null)
                {
                    bool isScopeFileUploaded = project.IsScopeFileUploaded;
                    bool isQuestionnaireFileUploaded = project.IsQuestionnaireFileUploaded;
                    bool isReportFileUploaded = project.IsReportFileUploaded;

                    return isScopeFileUploaded && isQuestionnaireFileUploaded && isReportFileUploaded;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex);
                return false;
            }
        }

        public async Task<bool> UpdateProject(ProjectData data)
        {
            if (data.Comments != null)
            {
                foreach (var comment in data.Comments)
                {
                    if (string.IsNullOrEmpty(comment.Text))
                    {
                        comment.CreatedAt = DateTime.Now;
                    }
                }
            }

            try
            {
                await Container.ReplaceItemAsync(data, data.id.ToString(), new PartitionKey(data.id.ToString()));
                Console.WriteLine("Item updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex);
                return false;
            }
        }

      
    }
}