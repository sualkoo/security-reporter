using webapi.Models;
using webapi.ProjectSearch.Models;

namespace webapi.Service
{
    public interface ICosmosService
    {
        Task<bool> AddProject(ProjectData data);
        Task<bool> AddProjectReport(ProjectReportData data);
        Task<List<ProjectData>> GetItems();
        Task<int> GetNumberOfProjects();
    }
}