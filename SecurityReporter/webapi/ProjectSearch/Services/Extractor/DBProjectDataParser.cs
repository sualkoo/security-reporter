using System.IO.Compression;
using webapi.ProjectSearch.Models;

namespace webapi.ProjectSearch.Services.Extractor
{
    public class DBProjectDataParser : IDBProjectDataParser
    {
        private ProjectReportData projectReportData;
        public byte[] Extract(ProjectReportData projectReportData)
        {
            this.projectReportData = projectReportData;
        }
    }
}
