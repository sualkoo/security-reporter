using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.Service;

public interface IAzureBlobService
{
    Task SaveReportPdf(byte[] pdfContent, Guid projectReportId, string projectReportName);
    Task<FileContentResult> GetReportPdf(Guid projectReportId);
    Task DeleteReportFolder(Guid projectReportId);
    Task SaveImagesFromZip(Guid projectReportId, List<Finding> findingsList);
    Task LoadImagesFromDb(Guid projectReportId, ProjectReportData projectReportData);
    Task UploadProjectFile(string filePath, string blobName);
    Task<bool> DownloadProject(string fileName, string path);
}