using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Service;

public interface IAzureBlobService
{
    Task SaveReportPdf(byte[] pdfContent, Guid projectReportId, string projectReportName);
    Task<FileContentResult> GetReportPdf(Guid projectReportId);
    Task DeleteReportFolder(Guid projectReportId);

}