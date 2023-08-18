using webapi.Login.Models;
using webapi.Models;
using webapi.ProjectSearch.Models;

namespace webapi.Service
{
    public interface ICosmosService
    {
        Task<bool> AddProject(ProjectData data);
        Task<bool> DeleteProject(string projectId);
        Task<List<string>> DeleteProjects(List<string> projectIds);
        Task<CountProjects> GetItems(int pageSize, int pageNumber, FilterData filter, SortData sort);
        //Task<int> GetNumberOfProjects();

        Task<bool> AddProjectReport(ProjectReportData data);
        Task<ProjectReportData> GetProjectReport(string projectId);
        Task<PagedDBResults<List<FindingResponse>>> GetPagedProjectReportFindings(string? projectName, string? details, string? impact, string? repeatability, string? references, string? cWE, string value, int page);

        Task<ProjectData> GetProjectById(string id);
        Task<bool> UpdateProject(ProjectData data);
        //Task<Profile> ProfileItems(string email);

        Task DeleteUsers();
        Task<UserRole> GetUserRole(string email);
    }
}
