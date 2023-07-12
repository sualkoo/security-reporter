using System;
using System.ComponentModel.DataAnnotations;
using webapi.Enums;
using webapi.Utils;

namespace webapi.Models;

public class ProjectData
{
    public Guid id { get; set; }
    public string ProjectName { get; set; }
    public ProjectStatus? ProjectStatus { get; set; }
    public ProjectQuestionare? ProjectQuestionare { get; set; }
    public ProjectScope? ProjectScope { get; set; }
    public ProjectDuration? ProjectDuration { get; set; }
    public DateOnly StartDate { get; set; }

    [DateRangeValidation("StartDate", ErrorMessage = "End date must be greater than or equal to start date.")]
    public DateOnly EndDate { get; set; }

    [DateRangeValidation("IKO", ErrorMessage = "IKO must be less than or equal to end date.")]
    public DateOnly IKO { get; set; } 

    [DateRangeValidation("TKO", ErrorMessage = "TKO must be less than or equal to end date.")]
    public DateOnly TKO { get; set; }
    public DateOnly RequestCreated { get; set; } 
    public List<Comment>? Commments { get; set; }
    public string? CatsNumber { get; set; }
    public ProjectOfferStatus? ProjectOfferStatus { get; set; }
    public string? PentestAspects { get; set; }
    public List<string>? WorkingTeam { get; set; }
    public string? ProjectLead { get; set; }
    public string? ReportStatus { get; set; }
    public List<string>? ContactForClients { get; set; } 
}
