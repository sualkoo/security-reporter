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

        [MinLength(1, ErrorMessage = "Description must have at least one item.")]
        public List<string>? Description { get; set; }
        //Boolean pole na Findings case scenarios
    }
}
