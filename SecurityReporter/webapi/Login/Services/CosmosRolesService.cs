
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
using System.Data;
using webapi.Login.Models;

namespace webapi.Service
{
    public class CosmosRolesService
    {
        private string PrimaryKey { get; set; }
        private string EndpointUri { get; } = "https://localhost:8081";
        private string DatabaseName { get; } = "ProjectRolesDatabase";
        private string ContainerName { get; } = "ProjectRolesContainer";
        private Microsoft.Azure.Cosmos.Container Container { get; }

        public CosmosRolesService(IConfiguration configuration)
        {
            PrimaryKey = configuration["DB:PrimaryKey"];
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
        }

        public CosmosRolesService(string primaryKey, string databaseId, string containerId, string cosmosEndpoint)
        {
            PrimaryKey = primaryKey;
            DatabaseName = databaseId;
            ContainerName = containerId;
            EndpointUri = cosmosEndpoint;
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            Container = cosmosClient.GetContainer(DatabaseName, ContainerName);
        }

        public async Task<string?> GetRole(string mail)
        {
            try
            {
                var queryText = $"SELECT VALUE r.Role FROM r WHERE r.id = '{mail}'";
                var queryDefinition = new QueryDefinition(queryText);

                using var resultSet = Container.GetItemQueryIterator<string>(queryDefinition);
                if (resultSet.HasMoreResults)
                {
                    var result = await resultSet.ReadNextAsync();
                    if (result.Any())
                    {
                        return result.FirstOrDefault();
                    }
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            return null;
        }

        public async Task<bool> AddRole(string mail, string role)
        {
            var document = new UserRole
            {
                id = mail,
                Role = role
            };

            try
            {
                ItemResponse<UserRole> response = await Container.CreateItemAsync(document);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return true;
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return false;
        }
    }
}
