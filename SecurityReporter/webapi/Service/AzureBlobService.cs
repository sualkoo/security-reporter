using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace webapi.Service;

public class AzureBlobService : IAzureBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobService(IConfiguration configuration)
    {
        var storageAccount = configuration["AzureStorage:StorageAccount"];
        var accessKey = configuration["AzureStorage:AccessKey"];

        if (string.IsNullOrEmpty(storageAccount) || string.IsNullOrEmpty(accessKey))
        {
            storageAccount = "sdastorage2023";
            accessKey = "eI7e7FRFosTNMQZTLR76SVBDE1fYh0pLNkBSiT7YnDJ3wLwcqnEDitIerlgu4kPi+ko//Be9dhw0+ASt3GOThg==";
        }
            
        var credential = new StorageSharedKeyCredential(storageAccount, accessKey);
        var blobUri = $"https://{storageAccount}.blob.core.windows.net";
        _blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
    }
}