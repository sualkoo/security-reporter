using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services
{
    public interface IProjectReportService
    {
        public Task<ProjectReportData> GetReportByIdAsync(Guid id);
        public Task<PagedDBResults<List<FindingResponse>>> GetReportFindingsAsync(string? projectName, string? details, string? impact, string? repeatability, string? references, string? cWE, int page);
        public Task<ProjectReportData> SaveReportFromZip(IFormFile file);
        public Task<bool> DeleteReportAsync(List<string> ids);
        public byte[] GetProjectZipFile();
    }
}
