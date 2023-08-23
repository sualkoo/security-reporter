using System.ComponentModel.DataAnnotations;

namespace webapi.ProjectSearch.Models.ProjectReport;

public class TestingMethodology : IEntity
{
    [MinLength(1, ErrorMessage = "ToolsUsed must have at least one item.")]
    public List<Tool>? ToolsUsed { get; set; }

    [MinLength(1, ErrorMessage = "AttackVectors must have at least one item.")]
    public List<string>? AttackVectors { get; set; }
}