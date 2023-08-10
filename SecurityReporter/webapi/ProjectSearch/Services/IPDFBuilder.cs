using Microsoft.AspNetCore.Mvc;

namespace webapi.ProjectSearch.Services;

public interface IPdfBuilder
{   
    Task<FileContentResult> GeneratePdfFromZip(Stream zipFileStream, Guid projectReportId);
}