using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.ProjectSearch.Models;

namespace webapi.Service
{
    public interface ICosmosService
    {
        Task<ProjectData> GetProjectById(string id);
        Task<bool> AddProject(ProjectData data);
        Task<bool> UpdateProject(ProjectData data);
        Task<bool> DeleteProject(string projectId);
        Task<List<string>> DeleteProjects(List<string> projectIds);
        Task<List<ProjectData>> GetItems(int pageSize, int pageNumber, FilterData filter);
        Task<int> GetNumberOfProjects();
        Task<bool> AddProjectReport(ProjectReportData data);
        Task<ProjectReportData> GetProjectReport(string projectId);
        Task<PagedDBResults<List<FindingResponse>>> GetPagedProjectReportFindings(string? projectName, string? details, string? impact, string? repeatability, string? references, string? cWE, int page);
        Task<bool> DeleteProjectReports(List<string> projectReportIds);
        Task<int[]> GetCriticalityData();
    }
}
