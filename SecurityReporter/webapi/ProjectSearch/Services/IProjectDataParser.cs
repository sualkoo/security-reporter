using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services
{
    public interface IProjectDataParser
    {
        public ProjectReportData Extract(Stream fileStream);
    }
}
