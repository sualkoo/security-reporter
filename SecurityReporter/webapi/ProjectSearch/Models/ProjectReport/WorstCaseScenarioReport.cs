using System.ComponentModel.DataAnnotations;

namespace webapi.ProjectSearch.Models.ProjectReport;

public class WorstCaseScenarioReport
{
    public WorstCaseScenarioReport()
    {
        WorstCaseReport = new List<List<bool>>();
    }

    [Required(ErrorMessage = "The finding description is required.")]
    public string FindingDescription { get; set; }

    [MinLength(1, ErrorMessage = "The worst case report must have at least one item.")]
    public List<List<bool>> WorstCaseReport { get; set; }
}