using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services.Extractor;

public interface IDbProjectDataParser
{
    public FileContentResult Extract(ProjectReportData projectReportData);
}