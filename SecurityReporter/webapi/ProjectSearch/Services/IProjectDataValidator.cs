using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services
{
    public interface IProjectDataValidator
    {
        public bool Validate(ProjectReportData projectReport);

    }
}
