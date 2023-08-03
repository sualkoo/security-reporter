using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services.Extractor
{
    public interface IDBProjectDataParser
    {
        public FileContentResult Extract(ProjectReportData projectReportData);
    }
}
