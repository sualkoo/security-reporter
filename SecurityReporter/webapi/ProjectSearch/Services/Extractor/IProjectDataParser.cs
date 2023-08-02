using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services.Extractor
{
    public interface IProjectDataParser
    {
        public ProjectReportData Extract(Stream fileStream);
    }
}
