using webapi.Models;

namespace webapi.Utils
{
    public interface IProjectDataValidation
    {
        bool IsValid(ProjectData data);
        void AddError(int index, string message);
    }
}
