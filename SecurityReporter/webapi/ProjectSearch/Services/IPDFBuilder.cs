using Microsoft.AspNetCore.Mvc;

namespace webapi.ProjectSearch.Services;

public interface IPDFBuilder
{   
    Task<FileContentResult> GeneratePDFFromZip(FileContentResult zipFile, string outputPDFname);
}