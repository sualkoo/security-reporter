using webapi.Enums;
using webapi.Utils;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models;

public class ProjectData
{
    public Guid id { get; set; }

    [StringValidation(ErrorMessage = "The Project Name field must not be empty or contain only whitespace.")]
    public string ProjectName { get; set; }
    
    [Range(0, 5, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectStatus? ProjectStatus { get; set; }

    [Range(0, 2, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectQuestionare? ProjectQuestionare { get; set; }

    [Range(0, 3, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectScope? ProjectScope { get; set; }

    [Range(0, 1, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectDuration? ProjectDuration { get; set; }
    public DateOnly StartDate { get; set; }

    [StartDateValidation("StartDate", ErrorMessage = "End date must be greater than or equal to start date.")]
    [TKOValidation("TKO", ErrorMessage = "End date must be greater than or equal to TKO.")]
    [IKOValidation("IKO", ErrorMessage = "End date must be greater than or equal to IKO.")]
    public DateOnly EndDate { get; set; }
    public DateOnly IKO { get; set; } 
    public DateOnly TKO { get; set; }

    [DateRangeValidation("EndDate", ErrorMessage = "End date must be less than or equal to request due date.")]
    public DateOnly RequestDue { get; set; }
    public DateOnly RequestCreated { get; set; } 
    public List<Comment>? Commments { get; set; }

    [StringValidation(ErrorMessage = "The Cats Number field must not be empty or contain only whitespace.")]
    public string? CatsNumber { get; set; }

    [Range(0, 9, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectOfferStatus? ProjectOfferStatus { get; set; }

    [StringValidation(ErrorMessage = "The Pentest Aspects field must not be empty or contain only whitespace.")]
    public string? PentestAspects { get; set; }

    [StringListValidation(ErrorMessage = "The Working Team fields must not be empty or contain only whitespace.")]
    public List<string>? WorkingTeam { get; set; }

    [StringValidation(ErrorMessage = "The Project Lead field must not be empty or contain only whitespace.")]
    public string? ProjectLead { get; set; }

    [StringValidation(ErrorMessage = "The Report Status field must not be empty or contain only whitespace.")]
    public string? ReportStatus { get; set; }

    [EmailListValidation(ErrorMessage = "The ContactForClients list must contain non-empty elements in email format.")]
    [DataType(DataType.EmailAddress)]
    public List<string>? ContactForClients { get; set; } 
}
