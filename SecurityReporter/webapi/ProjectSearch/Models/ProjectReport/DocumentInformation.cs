using System.ComponentModel.DataAnnotations;
using webapi.ProjectSearch.Models;

namespace webapi.Models.ProjectReport
{
    public class DocumentInformation : IEntity
    {
        [Required(ErrorMessage = "Project Report Name is required!")]
        public string? ProjectReportName { get; set; }
        [Required(ErrorMessage = "AssetType is required!")]
        public string? AssetType { get; set; }
        [Required(ErrorMessage = "Main author is required!")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "MainAuthor cannot contain numbers!")]
        public string? MainAuthor { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one author must be specified!")]
        public List<string>? Authors { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one reviewer must be specified!")]
        public List<string>? Reviewiers { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one approver must be specified!")]
        public List<string>? Approvers { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one ReportDocumentHistory must be specified!")]
        public List<ReportVersionEntry>? ReportDocumentHistory { get; set; }
        [Required(ErrorMessage = "The Report Date is required!")]
        [Range(typeof(DateTime), "1/1/2015", "31/12/2023", ErrorMessage = "The Report Date must be between {1} and {2}!")]
        public DateTime ReportDate { get; set; }
    }
}
