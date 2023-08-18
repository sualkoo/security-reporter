using System.ComponentModel.DataAnnotations;

namespace webapi.ProjectSearch.Models.ProjectReport;

public class ReportVersionEntry : IEntity
{
    [Required(ErrorMessage = "VersionDate is required!")]
    [DataType(DataType.Date)]
    public DateTime VersionDate { get; set; }

    [Required(ErrorMessage = "Version is required!")]
    [RegularExpression(@"^[0-9.]*$", ErrorMessage = "Version can only contain numbers and the dot character!")]
    public string? Version { get; set; }

    [Required(ErrorMessage = "WholeName is required.")]
    [StringLength(100, ErrorMessage = "WholeName cannot exceed 100 characters!")]
    public string? WholeName { get; set; }

    [Required(ErrorMessage = "ReportStatus is required!")]
    public string? ReportStatus { get; set; }
}