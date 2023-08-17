using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Models;

namespace webapi.Service;

public interface IAzureBlobService
{
    Task SaveReportPdf(byte[] pdfContent, Guid projectReportId, string projectReportName);
    Task<FileContentResult> GetReportPdf(Guid projectReportId);
    Task DeleteReportFolder(Guid projectReportId);
    Task SaveImagesFromZip(Guid projectReportId, List<Finding> findingsList);
    Task LoadImagesFromDB(Guid projectReportId, ProjectReportData projectReportData);

}