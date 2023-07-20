using System.ComponentModel.DataAnnotations;

namespace webapi.Models.ProjectReport
{
    public class WorstCaseScenarioReport
    {
        [Required(ErrorMessage = "The finding description is required.")]
        public string FindingDescription { get; set; }

        [MinLength(1, ErrorMessage = "The worst case report must have at least one item.")]
        public List<List<bool>> WorstCaseReport { get; set; }

        public WorstCaseScenarioReport()
        {
            this.WorstCaseReport = new List<List<bool>>();
        }
    }
}
