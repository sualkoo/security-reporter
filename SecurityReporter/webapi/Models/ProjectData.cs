using System.ComponentModel.DataAnnotations;
using webapi.Enums;
using webapi.Utils;

namespace webapi.Models;

public class ProjectData
{
    public Guid id { get; set; }

    [StringValidation(ErrorMessage = "The Project Name field must not be empty or contain only whitespace.")]
    public string ProjectName { get; set; }
    public ProjectStatus? ProjectStatus { get; set; }

    [CommentValidation(ErrorMessage = "The comment list field must have at least one object or it could be null.")]
    public List<Comment>? Commments { get; set; }

    [StringValidation(ErrorMessage = "The Cats Number field must not be empty or contain only whitespace.")]
    public string? CatsNumber { get; set; }
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
