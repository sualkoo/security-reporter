using webapi.Models;
using webapi.Service;

namespace webapi.Utils
{
    public class ProjectDataValidation : IProjectDataValidation
    {
        public List<ValidationError> ErrorList { get; set; }

        public bool IsValid(ProjectData data)
        {
            return true;
        }
    }
}
