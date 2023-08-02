using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DBTestingMethodologyExtractor
    {
        private TestingMethodology testingMethodology = null;
        public DBTestingMethodologyExtractor(TestingMethodology testingMethodology)
        {
            this.testingMethodology = testingMethodology;
        }
        public byte[] extractTestingMethodology()
        {
            return null;
        }
    }
}
