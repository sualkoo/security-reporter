using webapi.Enums;

namespace webapi.Models.ProjectReport
{
    public class Finding
    {
        public string? FindingAuthor { get; set; }
        public string? FindingName { get; set; }
        public List<string>? Location { get; set; }
        public string? Component { get; set; }
        public string? FoundWith { get; set; }
        public string? TestMethod { get; set; }
        public string? CVSS { get; set; }
        public string? CVSSVector { get; set; }
        public string? CWE { get; set; }
        public Criticality Criticality { get; set; }
        public Exploitability Exploitability { get; set; }
        public Category Category { get; set; }
        public Detectability Detectability { get; set; }
        public string? SubsectionDetails { get; set; }
        public string? SubsectionImpact { get; set; }
        public string? SubsectionRepeatability { get; set; }
        public string? SubsectionCountermeasures { get; set; }
        public List<string>? SubsectionReferences { get; set; }
    }
}
