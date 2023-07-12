using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;
using webapi.Models;

namespace webapi.Service

{
    public class DBWrapper
    {
        // TODO change me
        private readonly string endpointUri = "https://localhost:8081";
        private readonly string primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private readonly string databaseName = "ProjectDatabase";
        private readonly string containerName = "ProjectContainer";
        private Container container;

        public DBWrapper()
        {
            CosmosClient cosmosClient = new CosmosClient(endpointUri, primaryKey);
            this.container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<bool> AddProject(ProjectData data)
        {
            try
            {
                await container.CreateItemAsync(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}
