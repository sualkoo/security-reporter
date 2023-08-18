using System.ComponentModel.DataAnnotations;

namespace webapi.Models.ProjectReport
{
    public class TestingMethodology : IEntity
    {
        [Required(ErrorMessage = "ToolsUsed is required!")]
        public string? ToolsUsed { get; set; }

        [Required(ErrorMessage = "AttackVectors is required!")]
        public string? AttackVectors { get; set; }
    }
}
