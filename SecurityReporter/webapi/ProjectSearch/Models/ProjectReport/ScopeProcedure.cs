using System.ComponentModel.DataAnnotations;

namespace webapi.Models.ProjectReport
{
    public class ScopeProcedure
    {
        [Required(ErrorMessage = "Component is required.")]
        public string? Component { get; set; }

        [Required(ErrorMessage = "Detail is required.")]
        public string? Detail { get; set; }
    }
}
