using FluentAssertions;
using NUnit.Framework;
using System.Text;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract.Tests
{
    [TestFixture()]
    public class DBDocumentInformationExtractorTests
    {
        [Test()]
        public void extractDocumentInformationTest()
        {
            // Arrange
            DBDocumentInformationExtractor extractor = new DBDocumentInformationExtractor();

            // Input data
            DocumentInformation documentInfo = new DocumentInformation();
            documentInfo.ProjectReportName = "Dummy Project 1";
            documentInfo.AssetType = "Mobile Application";
            documentInfo.MainAuthor = "Lukas Nad";
            documentInfo.Authors = new List<string>();
            documentInfo.Authors.Add("Lukas Nad");
            documentInfo.Reviewiers = new List<string>();
            documentInfo.Reviewiers.Add("Katarina Amrichova");
            documentInfo.Approvers = new List<string>();
            documentInfo.Approvers.Add("Filip Mrocek");

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

            // Act
            var result = extractor.extractDocumentInformation(documentInfo);
            string resultDecoded = Encoding.UTF8.GetString(result);


            // Assert
            Assert.IsNotNull(result);
            StringAssert.Contains("\\newcommand{\\ReportProjectName}{Dummy Project 1\\xspace}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\ReportProjectType}{Penetration Test Report\\xspace}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\AssetType}{Mobile Application\\xspace}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\ReportDocumentReviewer}{Katarina Amrichova}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\ReportDocumentApprover}{Filip Mrocek}", resultDecoded);
            StringAssert.Contains("\t\\ReportVersionEntry{2023-08-02}{0.1}{Lukas Nad}{Initial Draft}", resultDecoded);
            StringAssert.Contains("\t\\ReportVersionEntry{2023-08-02}{0.2}{Michal Olencin}{Added Findings}", resultDecoded);
        }
    }
}