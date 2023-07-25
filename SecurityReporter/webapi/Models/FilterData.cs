using System.ComponentModel.DataAnnotations;
using webapi.Enums;
using webapi.Utils;

namespace webapi.Models
{
    public class FilterData
    {
        public string? FilteredProjectName { get; }
        public ProjectStatus? FilteredProjectStatus { get; }
        public ProjectQuestionare? FilteredProjectQuestionare { get; }
        public ProjectScope? FilteredProjectScope { get; }
        public int? FilteredPentestDurationStart { get; }
        public int? FilteredPentestDurationEnd { get; }
        public DateOnly? FilteredStartDate { get; }
        public DateOnly? FilteredEndDate { get; }
        public DateOnly? FilteredIKO { get; }
        public DateOnly? FilteredTKO { get; }
    }
}








