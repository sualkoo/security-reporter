using NUnit.Framework;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract.Tests
{
    [TestFixture()]
    public class DBDocumentInformationExtractorTests
    {
        [Test()]
        public void extractDocumentInformationTest()
        {
            // General info
            DBDocumentInformationExtractor extractor = new DBDocumentInformationExtractor();
            DocumentInformation documentInfo = new DocumentInformation();
            documentInfo.ProjectReportName = "Dummy Project 1";
            documentInfo.AssetType = "Mobile Application";
            documentInfo.MainAuthor = "Lukas Nad";


            // Authors

            // Reviewers
            documentInfo.Reviewiers = new List<string>();
            documentInfo.Reviewiers.Add("Katarina Amrichova");

            // Approvers
            documentInfo.Approvers = new List<string>();
            documentInfo.Approvers.Add("Filip Mrocek");


            // Report Version Entries
            var reportVersionEntry1 = new ReportVersionEntry();
            var reportVersionEntry2 = new ReportVersionEntry();

            reportVersionEntry1.VersionDate = DateTime.Now;
            reportVersionEntry1.Version = "0.1";
            reportVersionEntry1.WholeName = "Lukas Nad";
            reportVersionEntry1.ReportStatus = "Initial Draft";

            reportVersionEntry2.VersionDate = DateTime.Now;
            reportVersionEntry2.Version = "0.2";
            reportVersionEntry2.WholeName = "Michal Olencin";
            reportVersionEntry2.ReportStatus = "Added Findings";




            documentInfo.ReportDocumentHistory = new List<ReportVersionEntry>
            {
                reportVersionEntry1,
                reportVersionEntry2
            };

            extractor.extractDocumentInformation(documentInfo);
            Assert.Fail();
        }
    }
}