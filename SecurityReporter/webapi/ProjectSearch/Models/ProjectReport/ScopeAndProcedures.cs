using System.ComponentModel.DataAnnotations;
using webapi.ProjectSearch.Models;

namespace webapi.Models.ProjectReport
{
    public class ScopeAndProcedures : IEntity
    {
        [Required(ErrorMessage = "InScope list is required!")]
        [MinLength(1, ErrorMessage = "InScope must have at least one item.")]
        public List<ScopeProcedure>? InScope { get; set; }
        [Required(ErrorMessage = "InScope list is required!")]
        [MinLength(1, ErrorMessage = "OutOfScope must have at least one item.")]
        public List<ScopeProcedure>? OutOfScope { get; set; }
        [Required(ErrorMessage = "WorstCaseScenarios is required!")]
        [MinLength(1, ErrorMessage = "WorstCaseScenarios must have at least one item.")]
        public List<string>? WorstCaseScenarios { get; set; }
        [Required(ErrorMessage = "Environment is required!")]
        [MinLength(1, ErrorMessage = "Environment must have at least one item.")]
        public List<string>? Environment { get; set; }
        [Required(ErrorMessage = "WorstCaseScenariosReport is required!")]
        public WorstCaseScenarioReport? WorstCaseScenariosReport { get; set; }
    }
}
