using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.Models;
using webapi.ProjectSearch.Models;

namespace webapi.Service
{
    public interface ICosmosService
    {
        Task<bool> AddProject(ProjectData data);
        Task<bool> AddProjectReport(ProjectReportData data);
        Task<bool> DeleteProject(string projectId);
        Task<List<string>> DeleteProjects(List<string> projectIds);
        Task<List<ProjectData>> GetItems(); // check
        Task<List<ProjectData>> GetItems(int pageSize, int pageNumber, FilterData filter);
        Task<int> GetNumberOfProjects();
    }
}
