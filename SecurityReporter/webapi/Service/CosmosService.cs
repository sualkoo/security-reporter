﻿
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq.Expressions;
using webapi.Models;
using webapi.Models.ProjectReport;
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
            Logger = loggerFactory.CreateLogger<CosmosService>();
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
            catch (Exception)
            {
                Logger.LogError("An error occured while saving new Project Report to DB");
                return false;
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
            catch (Exception exception)
            {
                Logger.LogError("Report with searched ID not found");
                throw new Exception(exception.Message);
            }
        }
        public async Task<List<ProjectReportData>> GetProjectReports(string? subcategory, string keyword, string value)
        {
            List<ProjectReportData> results = new List<ProjectReportData>();
            string query = "SELECT * FROM c WHERE ";

            if (!string.IsNullOrEmpty(subcategory))
            {
                query = $"{query} c.{subcategory}.{keyword} LIKE @value";
            }
            else
            {
                query = $"{query} c.{keyword} LIKE @value";
            }

            try
            {
                Logger.LogInformation("Fetching reports from the database");
                QueryDefinition queryDefinition = new QueryDefinition(query).WithParameter("@value", $"%{value}%");
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
                throw new Exception("Error getting reports data from the Project Report Database" + exception);
            }
        }
    }
}
