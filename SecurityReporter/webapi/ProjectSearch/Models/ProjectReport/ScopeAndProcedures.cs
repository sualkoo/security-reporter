using System.ComponentModel.DataAnnotations;
using webapi.ProjectSearch.Models;

namespace webapi.Models.ProjectReport
{
    public class ScopeAndProcedures : IEntity
    {
        [MinLength(1, ErrorMessage = "InScope must have at least one item.")]
        public List<ScopeProcedure>? InScope { get; set; }

        [MinLength(1, ErrorMessage = "OutOfScope must have at least one item.")]
        public List<ScopeProcedure>? OutOfScope { get; set; }

        [MinLength(1, ErrorMessage = "WorstCaseScenarios must have at least one item.")]
        public List<string>? WorstCaseScenarios { get; set; }

        [MinLength(1, ErrorMessage = "Environment must have at least one item.")]
        public List<string>? Environment { get; set; }
        [Required(ErrorMessage = "WorstCaseScenariosReport is required!")]
        public WorstCaseScenarioReport? WorstCaseScenariosReport { get; set; }
    }
}
