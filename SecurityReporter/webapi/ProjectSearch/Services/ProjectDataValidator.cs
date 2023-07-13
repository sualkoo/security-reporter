using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services
{
    public class ProjectDataValidator
    {
        public bool Validate(ProjectReportData project)
        {
            if (project == null)
            {
                return false;
            }
            return true;
        }
    }
}
