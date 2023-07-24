using webapi.Models;
using webapi.ProjectSearch.Models;

namespace webapi.Service
{
    public interface ICosmosService
    {
        Task<bool> AddProject(ProjectData data);
        Task<bool> DeleteProject(string projectId);
        Task<List<string>> DeleteProjects(List<string> projectIds);
        Task<List<ProjectData>> GetItems(int pageSize, int pageNumber);
        Task<int> GetNumberOfProjects();

        Task<bool> AddProjectReport(ProjectReportData data);
        Task<ProjectReportData> GetProjectReport(string projectId);
        Task<List<ProjectReportData>> GetProjectReports(string subcategory, string keyword, string value);
        Task<PagedDBResults<List<ProjectReportData>>> GetPagedProjectReports(string? subcategory, string keyword, string value, int page);
        Task<PagedDBResults<List<ProjectReportData>>> GetPagedProjectReports(string? projectName, string? details, string? impact, string? repeatability, string? references, string? cWE, string value, int page);

    }
}