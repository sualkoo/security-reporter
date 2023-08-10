using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Service;

public interface IAzureBlobService
{
    Task SaveReportPdf(byte[] pdfContent, Guid projectReportId);
    Task<FileContentResult> GetReportPdf(Guid projectReportId);

}