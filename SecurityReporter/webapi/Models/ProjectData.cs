using webapi.Enums;
using webapi.Utils;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models;

public class ProjectData : ProjectList
{
    public DateTime? RequestCreated { get; set; }

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
