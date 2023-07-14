using System.ComponentModel.DataAnnotations;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Models
{
    public class ProjectReportData : IEntity
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Document info is required!")]
        public DocumentInformation? DocumentInfo { get; set; }
        [Required(ErrorMessage = "Executive Summary is required!")]
        public string? ExecutiveSummary { get; set; }
        public ProjectInformation? ProjectInfo { get; set; }
        public List<Finding>? Findings { get; set; }
        [Required(ErrorMessage = "ScopeAndProcedures is required!")]
        public ScopeAndProcedures? ScopeAndProcedures { get; set; }
        [Required(ErrorMessage = "TestingMethodology is required!")]
        public TestingMethodology? TestingMethodology { get; set; }
    }
} 