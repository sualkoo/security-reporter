using NUnit.Framework;
using webapi.ProjectSearch.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Tests
{
    [TestFixture()]
    public class DocumentInformationDeparserTests
    {
        [Test()]
        public void createDocumentInformationTest()
        {
            // General info
            DocumentInformationDeparser deparser = new DocumentInformationDeparser();
            DocumentInformation documentInfo = new DocumentInformation();
            documentInfo.ProjectReportName = "Dummy Project 1";
            documentInfo.AssetType = "Mobile Application";
            documentInfo.MainAuthor = "Lukas Nad";


            // Authors
            documentInfo.Authors = new List<string>();
            documentInfo.Authors.Add("Lukas Nad");
            documentInfo.Authors.Add("Michal Olencin");

            // Reviewers
            documentInfo.Reviewiers = new List<string>();
            documentInfo.Reviewiers.Add("Katarina Amrichova");

            // Approvers
            documentInfo.Approvers = new List<string>();
            documentInfo.Approvers.Add("Filip Mrocek");


            // Report Version Entries
            var reportVersionEntry1 = new ReportVersionEntry();

            reportVersionEntry1.VersionDate = DateTime.Now;
            reportVersionEntry1.Version = "0.1";
            reportVersionEntry1.WholeName = "Lukas Nad";
            reportVersionEntry1.ReportStatus = "Initial Draft";




            documentInfo.ReportDocumentHistory = new List<ReportVersionEntry>
            {
                reportVersionEntry1
            };

            deparser.createDocumentInformation(documentInfo);
            Assert.Fail();
        }
    }
}