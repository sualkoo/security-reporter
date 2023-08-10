using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;

namespace webapi.Service;

public class AzureBlobService : IAzureBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient projectReportContainerClient;
    private readonly ILogger _logger;

    public AzureBlobService(IConfiguration configuration, ILogger<AzureBlobService> logger)
    {
        _logger = logger;
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
        projectReportContainerClient = _blobServiceClient.GetBlobContainerClient("reports");
        if (!projectReportContainerClient.Exists())
        {
            projectReportContainerClient = _blobServiceClient.CreateBlobContainer("reports");
        }
    }


    public async Task SaveReportPdf(byte[] pdfContent, Guid projectReportId)
    {
        _logger.LogInformation("Saving generated PDF for report " + projectReportId);
        
        string filePath = $"{projectReportId}/Main.pdf";
        
        
        var pdfBlob = projectReportContainerClient.GetBlobClient(filePath);
        
        await pdfBlob.UploadAsync(new MemoryStream(pdfContent), true);
        _logger.LogInformation("Saved PDF blob to: " + pdfBlob.Uri);
    }

    public async Task<FileContentResult> GetReportPdf(Guid projectReportId)
    {
        _logger.LogInformation("Fetching PDF for report " + projectReportId);
        var pdfBlob = projectReportContainerClient.GetBlobClient($"{projectReportId}/Main.pdf");

        var pdfStream = await pdfBlob.OpenReadAsync();

        if (pdfStream == null)
        {
            throw new CustomException(StatusCodes.Status404NotFound, "PDF file not found.");
        }
        using var memoryStream = new MemoryStream();
        await pdfStream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var pdfBytes = memoryStream.ToArray();

        return new FileContentResult(pdfBytes, "application/pdf")
        {
            FileDownloadName = $"Main.pdf"
        };
    }
}