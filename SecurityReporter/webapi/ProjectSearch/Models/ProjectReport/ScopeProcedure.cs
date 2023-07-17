using System.ComponentModel.DataAnnotations;
using webapi.ProjectSearch.Models;

namespace webapi.Models.ProjectReport
{
    public class ScopeProcedure : IEntity
    {
        [Required(ErrorMessage = "Component is required.")]
        public string? Component { get; set; }

        [Required(ErrorMessage = "Detail is required.")]
        public string? Detail { get; set; }
    }
}
