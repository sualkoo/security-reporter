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
        Task<List<ProjectData>> GetItems(int pageSize, int pageNumber, string projectName = null, string projectStatus = null,
                                     string questionnaire = null, string projectScope = null, DateTime? startDate = null,
                                     DateTime? endDate = null, int? pentestDurationMin = null, int? pentestDurationMax = null,
                                     string ikoAndTKO = null);
        Task<int> GetNumberOfProjects();
    }
}