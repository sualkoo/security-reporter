using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DBDocumentInformationExtractor
    {
        private DocumentInformation documentInformation = null;
        public DBDocumentInformationExtractor(DocumentInformation documentInformation) 
        {
            this.documentInformation = documentInformation;
        }
        public byte[] extractDocumentInformation()
        {
            return null;
        }
    }
}
