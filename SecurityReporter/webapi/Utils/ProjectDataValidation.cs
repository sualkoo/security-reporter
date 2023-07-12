using webapi.Models;
using webapi.Service;
using webapi.Enums;
using System.ComponentModel;

namespace webapi.Utils
{
    public class ProjectDataValidation : IProjectDataValidation
    {
        public List<ValidationError> ErrorList { get; set; }

        public void AddError(int index, string message)
        {
            ErrorList.Add(
                new ValidationError
                {
                    FieldNumber = index,
                    ErrorMessage = message
                }
            );
        }

        public bool IsValid(ProjectData data)
        {
            return true;
        }
    }
}
