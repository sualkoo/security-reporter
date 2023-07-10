using webapi.Enums;

namespace webapi.Models;

public class Project
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; }
    public ProjectStatusEnum ProjectStatus { get; set; }
    public ProjectQuestionareEnum ProjectQuestionare { get; set; }
    public ProjectScopeEnum ProjectScope { get; set; }
    public ProjectDurationEnum ProjectDuration { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? IKO { get; set; }
    public DateOnly? TKO { get; set; }
    public DateOnly RequestCreated { get; set; }
    public List<Comment> Commments { get; set; }
    public int CatsNumber { get; set; }
    public ProjectOfferStatusEnum ProjectOfferStatus { get; set; }
    public string PentestAspects { get; set; }
    public List<string> WorkingTeam { get; set; }
    public string ProjectLead { get; set; }
    public string ReportStatus { get; set; }
    public List<string> ContactForClients { get; set; }

}
