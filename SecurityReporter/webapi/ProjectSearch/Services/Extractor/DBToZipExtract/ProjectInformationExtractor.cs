using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class ProjectInformationExtractor
    {
        private ProjectInformation projectInformation = null;
        public ProjectInformationExtractor(ProjectInformation projectInformation)
        {
            this.projectInformation = projectInformation;
        }
        public byte[] extractProjectInformation()
        {
            return null;
        }
    }
}
