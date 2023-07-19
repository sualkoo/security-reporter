using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services
{
    public interface IProjectReportService
    {
        public Task<ProjectReportData> GetReportByIdAsync(Guid id);
        public Task<List<ProjectReportData>> GetReportsAsync(string? subcategory, string keyword, string value);
        public Task<ProjectReportData> SaveReportFromZip(IFormFile file);
    }
}
