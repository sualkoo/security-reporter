namespace webapi.Models.ProjectReport
{
    public class ReportVersionEntry
    {
        public DateOnly VersionDate { get; set; }
        public string? Version { get; set; }
        public string? WholeName { get; set; }
        public string? ReportStatus { get; set; }
    }
}
