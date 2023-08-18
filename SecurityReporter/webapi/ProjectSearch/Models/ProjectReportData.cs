using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Models;

public class ProjectReportData : IEntity
{
    [JsonProperty("id")] public Guid Id { get; set; }

    [Required(ErrorMessage = "Document info is required!")]
    public DocumentInformation? DocumentInfo { get; set; }

    [Required(ErrorMessage = "Executive Summary is required!")]
    public string? ExecutiveSummary { get; set; }

    [Required(ErrorMessage = "ProjectInfo is required!")]
    public ProjectInformation? ProjectInfo { get; set; }

    [MinLength(1, ErrorMessage = "Findings must have at least one item.")]
    public List<Finding>? Findings { get; set; }

    [Required(ErrorMessage = "ScopeAndProcedures is required!")]
    public ScopeAndProcedures? ScopeAndProcedures { get; set; }

    [Required(ErrorMessage = "TestingMethodology is required!")]
    public TestingMethodology? TestingMethodology { get; set; }

    public string? FindingsDatabase { get; set; }
    public DateTime uploadDate { get; set; }
}