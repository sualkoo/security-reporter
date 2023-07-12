using System.ComponentModel.DataAnnotations;
using webapi.Enums;

namespace webapi.Models;

public class ProjectData
{
    public Guid id { get; set; }
    public string ProjectName { get; set; }
    
    [Range(0, 5, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectStatus? ProjectStatus { get; set; }

    [Range(0, 2, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectQuestionare? ProjectQuestionare { get; set; }

    [Range(0, 3, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectScope? ProjectScope { get; set; }

    [Range(0, 1, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectDuration? ProjectDuration { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? IKO { get; set; }
    public DateOnly? TKO { get; set; }
    public DateOnly RequestCreated { get; set; }
    public List<Comment>? Commments { get; set; }
    public string? CatsNumber { get; set; }

    [Range(0, 9, ErrorMessage = "Value for attribute {0} must be between {1} and {2}.")]
    public ProjectOfferStatus? ProjectOfferStatus { get; set; }
    public string? PentestAspects { get; set; }
    public List<string>? WorkingTeam { get; set; }
    public string? ProjectLead { get; set; }
    public string? ReportStatus { get; set; }
    public List<string>? ContactForClients { get; set; } 
}
