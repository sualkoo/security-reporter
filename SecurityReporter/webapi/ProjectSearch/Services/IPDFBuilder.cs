using Microsoft.AspNetCore.Mvc;

namespace webapi.ProjectSearch.Services;

public interface IPDFBuilder
{   
    Task<FileContentResult> GeneratePDFFromZip(Stream zipFileStream, string outputPDFname);
}