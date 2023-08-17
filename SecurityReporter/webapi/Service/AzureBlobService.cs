using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.ProjectReport;
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


    public async Task SaveReportPdf(byte[] pdfContent, Guid projectReportId, string projectReportName)
    {
        _logger.LogInformation("Saving generated PDF for report " + projectReportId);
        
        string filePath = $"{projectReportId}/{projectReportName.Replace(" ", "_")}.pdf";
        
        
        var pdfBlob = projectReportContainerClient.GetBlobClient(filePath);
        
        await pdfBlob.UploadAsync(new MemoryStream(pdfContent), true);
        _logger.LogInformation("Saved PDF blob to: " + pdfBlob.Uri);
    }

    public async Task<FileContentResult> GetReportPdf(Guid projectReportId)
    {
        _logger.LogInformation("Fetching PDF for report " + projectReportId);
    
        // List the blobs in the specified directory
        List<BlobItem> blobs = new List<BlobItem>();
        await foreach (BlobHierarchyItem blob in projectReportContainerClient.GetBlobsByHierarchyAsync(
                           delimiter: "/",
                           prefix: $"{projectReportId}/"))
        {
            if (blob.IsBlob)
            {
                blobs.Add(blob.Blob);
            }
        }
    
        // Find the first PDF blob in the list
        var pdfBlob = blobs.FirstOrDefault(blob => blob.Name.EndsWith(".pdf"));
    
        if (pdfBlob == null)
        {
            throw new CustomException(StatusCodes.Status404NotFound, "PDF file not found.");
        }

        var pdfBlobClient = projectReportContainerClient.GetBlobClient(pdfBlob.Name);
        BlobDownloadInfo pdfDownloadInfo = await pdfBlobClient.DownloadAsync();
    
        using var memoryStream = new MemoryStream();
        await pdfDownloadInfo.Content.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var pdfBytes = memoryStream.ToArray();
        
        return new FileContentResult(pdfBytes, "application/pdf")
        {
            FileDownloadName = pdfBlobClient.Name.Substring(37)
        };
    }

    public async Task DeleteReportFolder(Guid projectReportId)
    {
        _logger.LogInformation("Deleting files for report " + projectReportId);
        await foreach (BlobHierarchyItem blob in projectReportContainerClient.GetBlobsByHierarchyAsync(
                           delimiter: "/",
                           prefix: $"{projectReportId}/"))
        {
            if (blob.IsBlob)
            {
                projectReportContainerClient.DeleteBlob(blob.Blob.Name);
            }
        }
    }

    public async Task LoadImagesFromDB(Guid projectReportId, ProjectReportData projectReportData)
    {
        _logger.LogInformation("Creating images for the ZIP file " + projectReportId);
        
        foreach(Finding finding in projectReportData.Findings)
        {
            await foreach (BlobHierarchyItem blob in projectReportContainerClient.GetBlobsByHierarchyAsync(
                                       delimiter: "/",
                                       prefix: $"{projectReportId}/Findings/{finding.FolderName}/"))
            {
                BlobClient blobClient = projectReportContainerClient.GetBlobClient(blob.Blob.Name);

                BlobDownloadInfo download = await blobClient.DownloadAsync();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await download.Content.CopyToAsync(memoryStream);
                    byte[] byteArray = memoryStream.ToArray();

                    string blobName = blob.Blob.Name; 
                    finding.addImage(blobName.Substring(($"{projectReportId}/Findings/{finding.FolderName}/").Length), byteArray);
                }
            }
        }
    }

    public async Task SaveImagesFromZip(Guid projectReportId, List<Finding> findingsList)
    {
        _logger.LogInformation("Converting images into blobs " + projectReportId);
        foreach(Finding finding in findingsList)
        {
            foreach(FileData imageData in finding.getImages())
            {
                string blobPath = $"{projectReportId}/Findings/{finding.FolderName}/{imageData.FileName}";
                BlobClient blobClient = projectReportContainerClient.GetBlobClient(blobPath);
                using(MemoryStream stream = new MemoryStream(imageData.Content))
                {
                    await blobClient.UploadAsync(stream);
                    Console.WriteLine(blobClient.Uri);
                }
            }
            finding.clearImageList();
        }
    }
}