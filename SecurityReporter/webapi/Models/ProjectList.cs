using webapi.Enums;
using webapi.Utils;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models;

public class ProjectList
{
    public Guid id { get; set; }

    [StringValidation(ErrorMessage = "The Project Name field must not be empty or contain only whitespace.")]
    public string ProjectName { get; set; }

    [Range(1, 6, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectStatus? ProjectStatus { get; set; }

    [Range(1, 3, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectQuestionare? ProjectQuestionare { get; set; }

    [Range(1, 4, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectScope? ProjectScope { get; set; }

    [Range(2, 10, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public int? PentestDuration { get; set; }
    public DateOnly? StartDate { get; set; }

    [StartDateValidation("StartDate", ErrorMessage = "End date must be greater than or equal to start date.")]
    [TKOValidation("TKO", ErrorMessage = "End date must be greater than or equal to TKO.")]
    [IKOValidation("IKO", ErrorMessage = "End date must be greater than or equal to IKO.")]
    public DateOnly? EndDate { get; set; }
    public DateOnly? IKO { get; set; }
    public DateOnly? TKO { get; set; }
    public List<Comment>? Comments { get; set; }

    [DateRangeValidation("EndDate", ErrorMessage = "End date must be less than or equal to report due date.")]
    public DateOnly? ReportDueDate { get; set; }
}
