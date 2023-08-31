# Introduction

Welcome to the documentation for SecurityReporter, a C# and .NET-based application for managing cybersecurity assessment reports and projects. Follow the instructions below to properly set up and configure the application.


## Prerequisites

- **texlive-full**: The application depends on `texlive-full` for generating PDFs. It needs to run on Linux, Windows is currently not supported. 
- You can download and install it using the following command for debian linux:

  ```bash
  sudo apt-get install texlive-full
  ```

- **Node.js**: The application uses Node.js for server enviroment.

- **Visual Studio with ASP.NET and web development extension**: IDE and extension for developing project.

- **Azure Cosmos DB**: The application uses Cosmos DB to store its data. Create a database named `ProjectDatabase` and two containers: `ProjectContainer` and `ReportContainer` in your Cosmos DB account.

- **Azure Blob Service**: Configure the `appsettings.json` or user secrets for integration with Azure Blob Storage. Create a container named `reports` in your Azure Blob Storage account.

# Project setup

1. Clone repository

2. Download and install Cosmos DB Emulator:

   - https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21

3. Open Cosmos DB Emulator (emulator should show up in browser) and create Database called ProjectDatabase with these Containers all with id as Partition key:
   - ProjectContainer
   - ReportContainer
   - ProjectRolesContainer

4. For now you need to add users with roles manually to the ProjectRolesContainer in this format:
   - Example: 
      - {
               "id": "client@client.sk",
               "Role": "client"
         }
      - Attribute Role can be admin/client/coordinator/pentester/default.
      - You can add new item by clicking on ProjectRolesDatabase -> ProjectRolesContainer -> Items, then click on New item and then click save.
      - If you want to login password is same as login email. (Example: username: client@client.sk, password: client@client.sk)

5. Configure the application by updating the `appsettings.json` file or user secrets  with the appropriate values, if you don't specify configuration for CosmosDB or AzureStorage, the application will use production connection by default:

   ```json
   {
      "CosmosDB": {
         "EndpointUri": "your-cosmosdb-endpoint-uri",
         "PrimaryKey": "your-cosmosdb-primary-key"
      },
      "AzureStorage": {
         "StorageAccount" : "your-azure-storage-account",
         "AccessKey": "your-azure-storage-access-key"
      }
   }
   ```
   [ASP.NET Safe storage](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows)

6. Open repository folder in terminal and type:
   ```
   npm install
   ```

   ```
   npm i bootstrap-icons --save
   ```

7. Open project solution in Visual Studio
   - In View tab click on Solution explorer.
   - In Solution explorer right click on Solution and choose Properties
   - In menu select Common Properties -> Startup Project
   - Click on Multiple startup project radio button
   - In table set webapi Project will be on top (select it and move it with arrows on right) and select Start Action in right column
   - In table set angularapp Project to second position and select Start Action in right column
   - Click Apply and OK

8. Start project (F5)

9. Swagger and angular page should be visible in browser.

# Contribute

You can contribute to SecurityReporter to help make it even better! Follow these guides to get started:

- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)


