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
        private readonly string projectContainerName = "ProjectContainer";
        private readonly string reportContainerName = "ProjectReportContainer";

        private Container projectContainer;
        private Container reportContainer;


        public DBWrapper()
        {
            CosmosClient cosmosClient = new CosmosClient(endpointUri, primaryKey);
            this.projectContainer = cosmosClient.GetContainer(databaseName, projectContainerName);
            this.reportContainer = cosmosClient.GetContainer(databaseName, reportContainerName);
        }

        public async Task<bool> AddProject(ProjectData data)
        {
            try
            {
                await projectContainer.CreateItemAsync(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddProjectReport(ProjectReportData data)
        {
            try
            {
                await reportContainer.CreateItemAsync(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}
