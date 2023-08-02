using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DBExecutiveSummaryExtractor
    {
        private string executiveSummary = "";
        public DBExecutiveSummaryExtractor(string executiveSummary)
        {
            this.executiveSummary = executiveSummary;
        }
        public byte[] extractExecutiveSummary()
        {
            return null;
        }
    }
}
