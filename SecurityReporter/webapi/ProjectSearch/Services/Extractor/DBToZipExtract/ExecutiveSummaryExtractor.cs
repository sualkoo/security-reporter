using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class ExecutiveSummaryExtractor
    {
        private string executiveSummary = "";
        public ExecutiveSummaryExtractor(string executiveSummary)
        {
            this.executiveSummary = executiveSummary;
        }
        public byte[] extractExecutiveSummary()
        {
            return null;
        }
    }
}
