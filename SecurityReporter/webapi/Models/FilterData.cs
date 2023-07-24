using System.ComponentModel.DataAnnotations;
using webapi.Enums;
using webapi.Utils;

namespace webapi.Models
{
    public class FilterData
    {
        public string? FilteredProjectName { get; set; }
        public ProjectStatus? FilteredProjectStatus { get; set; }
        public ProjectQuestionare? FilteredProjectQuestionare { get; set; }
        public ProjectScope? FilteredProjectScope { get; set; }
        public int? FilteredPentestDuration { get; set; }
        public DateOnly? FilteredStartDate { get; set; }
        public DateOnly? FilteredEndDate { get; set; }
        public DateOnly? FilteredIKO { get; set; }
        public DateOnly? FilteredTKO { get; set; }
    }
}








