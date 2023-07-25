using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
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
        private Microsoft.Azure.Cosmos.Container Container { get; }

        public CosmosService(IConfiguration configuration)
        {
            PrimaryKey = configuration["DB:PrimaryKey"];
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
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

        public Task<bool> AddProjectReport(ProjectReportData data)
        {
            throw new NotImplementedException();
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

        public async Task<List<ProjectData>> GetItems(int pageSize, int pageNumber, [FromQuery] FilterData filter)
        {
            // Calculate the number of items to skip based on the pageSize and pageNumber
            int skipCount = pageSize * (pageNumber - 1);

            // Build the Cosmos DB SQL query based on the provided filter and pagination parameters
            var queryBuilder = new StringBuilder("SELECT * FROM c WHERE 1=1");

            if (!string.IsNullOrWhiteSpace(filter.FilteredProjectName))
            {
                queryBuilder.Append($" AND c.ProjectName = '{filter.FilteredProjectName}'");
            }

            if (filter.FilteredProjectStatus.HasValue)
            {
                queryBuilder.Append($" AND c.ProjectStatus = {(int)filter.FilteredProjectStatus.Value}");
            }

            if (filter.FilteredProjectQuestionare.HasValue)
            {
                queryBuilder.Append($" AND c.ProjectQuestionare = {(int)filter.FilteredProjectQuestionare.Value}");
            }

            if (filter.FilteredProjectScope.HasValue)
            {
                queryBuilder.Append($" AND c.ProjectScope = {(int)filter.FilteredProjectScope.Value}");
            }

            if (filter.FilteredPentestDurationStart.HasValue)
            {
                queryBuilder.Append($" AND c.PentestDuration >= {filter.FilteredPentestDurationStart.Value}");
            }

            if (filter.FilteredPentestDurationEnd.HasValue)
            {
                queryBuilder.Append($" AND c.PentestDuration <= {filter.FilteredPentestDurationEnd.Value}");
            }

            if (filter.FilteredStartDate.HasValue)
            {
                queryBuilder.Append($" AND c.StartDate >= '{filter.FilteredStartDate.Value.ToString("yyyy-MM-dd")}'");
            }

            if (filter.FilteredEndDate.HasValue)
            {
                queryBuilder.Append($" AND c.EndDate <= '{filter.FilteredEndDate.Value.ToString("yyyy-MM-dd")}'");
            }

            if (filter.FilteredIKO.HasValue)
            {
                queryBuilder.Append($" AND c.IKO = '{filter.FilteredIKO.Value.ToString("yyyy-MM-dd")}'");
            }

            if (filter.FilteredTKO.HasValue)
            {
                queryBuilder.Append($" AND c.TKO = '{filter.FilteredTKO.Value.ToString("yyyy-MM-dd")}'");
            }

            // Add OFFSET and LIMIT parameters for pagination
            queryBuilder.Append($" OFFSET {skipCount} LIMIT {pageSize}");

            // Print the constructed query for debugging purposes
            Console.WriteLine("Constructed query: " + queryBuilder.ToString());

            // Execute the query and retrieve the filtered projects
            var filteredProjects = new List<ProjectData>();

            try
            {
                FeedIterator<ProjectData> resultSetIterator = Container.GetItemQueryIterator<ProjectData>(new QueryDefinition(queryBuilder.ToString()));

                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<ProjectData> response = await resultSetIterator.ReadNextAsync();
                    filteredProjects.AddRange(response.Resource);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while filtering projects from DB: " + ex);
                throw;
            }

            return filteredProjects;
        }
    }
}
