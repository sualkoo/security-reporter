using System.ComponentModel.DataAnnotations;

namespace webapi.ProjectSearch.Models.ProjectReport;

public class ScopeProcedure : IEntity
{
    [Required(ErrorMessage = "Component is required.")]
    public string? Component { get; set; }

    [Required(ErrorMessage = "Detail is required.")]
    public string? Detail { get; set; }
}