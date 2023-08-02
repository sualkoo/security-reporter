using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DBFindingsExtractor
    {
        private Finding finding = null;
        public DBFindingsExtractor(Finding finding)
        {
            this.finding = finding;
        }
        public byte[] extractFindings()
        {
            return null;
        }
    }
}
