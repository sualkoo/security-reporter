using webapi.Models;
using webapi.Service;
using webapi.Enums;
using System.ComponentModel;

namespace webapi.ProjectManagement.Utils
{
    public class ProjectDataValidation : IProjectDataValidation
    {
        public List<ValidationError> ErrorList = new List<ValidationError>();

        public void AddError(string field, string message)
        {
            ErrorList.Add(
                new ValidationError
                {
                    FieldName = field,
                    ErrorMessage = message
                }
            );
        }

        public bool IsValid(ProjectData data)
        {
            if (ErrorList.Count > 0)
            {
                return false;
            }

            return true;
        }
    }
}
