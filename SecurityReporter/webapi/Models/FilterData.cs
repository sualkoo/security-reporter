using System.ComponentModel.DataAnnotations;
using webapi.Enums;
using webapi.Utils;

namespace webapi.Models
{
    public class FilterData
    {

        [StringValidation(ErrorMessage = "The Project Name field must not be empty or contain only whitespace.")]
        public string FilteredProjectName { get; set; }

        [Range(0, 5, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
        public ProjectStatus? FilteredProjectStatus { get; set; }

        [Range(0, 2, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
        public ProjectQuestionare? FilteredProjectQuestionare { get; set; }

        [Range(0, 3, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
        public ProjectScope? FilteredProjectScope { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
        public int? FilteredPentestDuration { get; set; }
        public DateOnly? FilteredStartDate { get; set; }

        [StartDateValidation("StartDate", ErrorMessage = "End date must be greater than or equal to start date.")]
        [TKOValidation("TKO", ErrorMessage = "End date must be greater than or equal to TKO.")]
        [IKOValidation("IKO", ErrorMessage = "End date must be greater than or equal to IKO.")]
        public DateOnly? FilteredEndDate { get; set; }
        public DateOnly? FilteredIKO { get; set; }
        public DateOnly? FilteredTKO { get; set; }
    }
}