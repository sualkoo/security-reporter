using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Models;

public class FindingResponse
{
    public Guid ProjectReportId { get; set; }
    public string ProjectReportName { get; set; }
    public Finding Finding { get; set; }
}