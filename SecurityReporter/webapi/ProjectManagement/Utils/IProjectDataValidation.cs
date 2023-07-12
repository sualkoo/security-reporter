using webapi.Models;

namespace webapi.ProjectManagement.Utils
{
    public interface IProjectDataValidation
    {
        bool IsValid(ProjectData data);
        void AddError(string index, string message);
    }
}
