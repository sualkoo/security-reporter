using webapi.Models;
using webapi.ProjectSearch.Models;

namespace webapi.Service
{
    public interface ICosmosService
    {
        Task<bool> AddProject(ProjectData data);
        Task<bool> AddProjectReport(ProjectReportData data);
        Task<ProjectReportData> GetProjectReport(string projectId);
        Task<List<ProjectReportData>> GetProjectReports(string subcategory, string keyword, string value);
        Task<PagedDBResults<List<ProjectReportData>>> GetPagedProjectReports(string? subcategory, string keyword, string value, int page);
    }
}