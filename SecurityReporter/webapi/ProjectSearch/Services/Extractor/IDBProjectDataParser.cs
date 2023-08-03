using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services.Extractor
{
    public interface IDBProjectDataParser
    {
        public byte[] Extract(ProjectReportData projectReportData);
    }
}
