using System.ComponentModel.DataAnnotations;

namespace webapi.Models.ProjectReport
{
    public class ScopeAndProcedures : IEntity
    {
        [Required(ErrorMessage = "InScope list is required!")]
        public string? InScope { get; set; }
        [Required(ErrorMessage = "OutOfScope list is required!")]
        public string? OutOfScope { get; set; }
        [Required(ErrorMessage = "WorstCaseScenarios is required!")]
        public string? WorstCaseScenarios { get; set; }
        [Required(ErrorMessage = "Environment is required!")]
        public string? Environment { get; set; }
        [Required(ErrorMessage = "WorstCaseScenariosReport is required!")]
        public string? WorstCaseScenariosReport { get; set; }
    }
}
