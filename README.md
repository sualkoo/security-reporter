# SecurityReporter readme

Welcome to the documentation for SecurityReporter, a C# and .NET-based application for managing cybersecurity assessment reports and projects. Follow the instructions below to properly set up and configure the application.

## Prerequisites

- **texlive-full**: The application depends on `texlive-full` for generating PDFs. If it's not already installed on your server. Generating of PDFs is disabled by default, you can enable it in configuration by adding
  "GeneratePdfsFromReports" to true.
- You can download and install it using the following command for debian linux:

  ```bash
  sudo apt-get install texlive-full
  ```

- **Azure Cosmos DB**: The application uses Cosmos DB to store its data. Create a database named `ProjectDatabase` and two containers: `ProjectContainer` and `ReportContainer` in your Cosmos DB account.

- **Azure Blob Service**: Configure the `appsettings.json` or user secrets for integration with Azure Blob Storage. Create a container named `reports` in your Azure Blob Storage account.

## Configuration

Configure the application by updating the `appsettings.json` file or user secrets with the appropriate values, if you don't specify configuration for CosmosDB or AzureStorage, the application will use production connection by default:

```json
{
  "CosmosDB": {
    "EndpointUri": "your-cosmosdb-endpoint-uri",
    "PrimaryKey": "your-cosmosdb-primary-key"
  },
  "AzureStorage" {
      "StorageAccount" : "your-azure-storage-account",
      "AccessKey": "your-azure-storage-access-key"

  },
  "GeneratePdfsFromReports" : false
}
```
