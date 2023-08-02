using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DBProjectInformationExtractor
    {
        private ProjectInformation projectInformation = null;
        public DBProjectInformationExtractor(ProjectInformation projectInformation)
        {
            this.projectInformation = projectInformation;
        }
        public byte[] extractProjectInformation()
        {
            return null;
        }
    }
}
