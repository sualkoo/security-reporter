using webapi.Enums;

namespace webapi.Models;

public class ProjectData
{
    public Guid id { get; set; }
    public string ProjectName { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public ProjectQuestionare ProjectQuestionare { get; set; }
    public ProjectScope ProjectScope { get; set; }
    public ProjectDuration ProjectDuration { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? IKO { get; set; }
    public DateOnly? TKO { get; set; }
    public DateOnly RequestCreated { get; set; }
    public List<Comment> Commments { get; set; }
    public int CatsNumber { get; set; }
    public ProjectOfferStatus ProjectOfferStatus { get; set; }
    public string PentestAspects { get; set; }
    public List<string> WorkingTeam { get; set; }
    public string ProjectLead { get; set; }
    public string ReportStatus { get; set; }
    public List<string> ContactForClients { get; set; }

}
