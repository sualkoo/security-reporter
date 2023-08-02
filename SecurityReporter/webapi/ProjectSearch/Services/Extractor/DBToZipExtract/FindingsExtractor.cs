using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class FindingsExtractor
    {
        private Finding finding = null;
        public FindingsExtractor(Finding finding)
        {
            this.finding = finding;
        }
        public byte[] extractFindings()
        {
            return null;
        }
    }
}
