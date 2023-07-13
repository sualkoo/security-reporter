using Newtonsoft.Json;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Models
{
    public class ProjectReportData
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        public DocumentInformation? DocumentInfo { get; set; }
        public string? ExecutiveSummary { get; set; }
        public ProjectInformation? ProjectInfo { get; set; }
        public List<Finding>? Findings { get; set; }
        public ScopeAndProcedures? ScopeAndProcedures { get; set; }
        public TestingMethodology? TestingMethodology { get; set; }
    }
} 