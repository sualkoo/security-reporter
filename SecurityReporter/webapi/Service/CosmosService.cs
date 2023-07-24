﻿using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public async Task<List<ProjectData>> GetItems(int pageSize, int pageNumber, string projectName = null, string projectStatus = null,
                                              string questionnaire = null, string projectScope = null, DateTime? startDate = null,
                                              DateTime? endDate = null, int? pentestDurationMin = null, int? pentestDurationMax = null,
                                              string ikoAndTKO = null)
        {
            int skipCount = pageSize * (pageNumber - 1);
            int itemCount = pageSize;

            // Build the base query
            var queryString = "SELECT * FROM c";
            var queryParameters = new Dictionary<string, object>
    {

    };

            // Create a list to hold the filter conditions
            var filterConditions = new List<string>();

            // Filter based on Project Name
            if (!string.IsNullOrEmpty(projectName))
            {
                filterConditions.Add($"CONTAINS(LOWER(c.ProjectName), @projectName)");
                queryParameters["@projectName"] = projectName.ToLower();
            }

            // Filter based on Project Status
            if (!string.IsNullOrEmpty(projectStatus))
            {
                filterConditions.Add("c.ProjectStatus = @projectStatus");
                queryParameters["@projectStatus"] = projectStatus;
            }

            // Filter based on Questionnaire
            if (!string.IsNullOrEmpty(questionnaire))
            {
                filterConditions.Add("c.Questionnaire = @questionnaire");
                queryParameters["@questionnaire"] = questionnaire;
            }

            // Filter based on Project Scope
            if (!string.IsNullOrEmpty(projectScope))
            {
                filterConditions.Add("c.ProjectScope = @projectScope");
                queryParameters["@projectScope"] = projectScope;
            }

            // Filter based on Start Date
            if (startDate.HasValue)
            {
                filterConditions.Add("c.StartDate >= @startDate");
                queryParameters["@startDate"] = startDate.Value;
            }

            // Filter based on End Date
            if (endDate.HasValue)
            {
                filterConditions.Add("c.EndDate <= @endDate");
                queryParameters["@endDate"] = endDate.Value;
            }

            // Filter based on Pentest Duration (within a range)
            if (pentestDurationMin.HasValue && pentestDurationMax.HasValue)
            {
                filterConditions.Add("c.PentestDuration >= @pentestDurationMin AND c.PentestDuration <= @pentestDurationMax");
                queryParameters["@pentestDurationMin"] = pentestDurationMin.Value;
                queryParameters["@pentestDurationMax"] = pentestDurationMax.Value;
            }

            // Filter based on IKO & TKO
            if (!string.IsNullOrEmpty(ikoAndTKO))
            {
                filterConditions.Add("c.IKO_TKO = @ikoAndTKO");
                queryParameters["@ikoAndTKO"] = ikoAndTKO;
            }

            // Combine the filter conditions into the final query
            if (filterConditions.Count > 0)
            {
                queryString += " AND " + string.Join(" AND ", filterConditions);
            }

            // Add OFFSET and LIMIT parameters
            queryString += " OFFSET @skipCount LIMIT @itemCount";
            queryParameters["@skipCount"] = skipCount;
            queryParameters["@itemCount"] = itemCount;

            // Execute the query and return the filtered items
            var items = new List<ProjectData>();
            var queryDefinition = new QueryDefinition(queryString);
            foreach (var param in queryParameters)
            {
                queryDefinition.WithParameter(param.Key, param.Value);
            }
            var resultSetIterator = Container.GetItemQueryIterator<ProjectData>(queryDefinition);

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
    }
}
