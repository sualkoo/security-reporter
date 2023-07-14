namespace webapi.Models.ProjectReport
{
    public class DocumentInformation
    {
        public string? ProjectReportName { get; set; }
        public string? AssetType { get; set; }
        public string? MainAuthor { get; set; }
        public List<string>? Authors { get; set; }
        public List<string>? Reviewiers { get; set; }
        public List<string>? Approvers { get; set; }
        public List<ReportVersionEntry>? ReportDocumentHistory { get; set; }
        public DateOnly ReportDate { get; set; }
    }
}
