using FluentAssertions;
using NUnit.Framework;
using System.IO.Compression;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Tests
{
    [TestFixture()]
    public class DocumentInformationExtractorTests
    {
        private ZipArchive zipArchive;


        [SetUp]
        public void SetUp()
        {
            zipArchive = ZipFile.OpenRead("../../../ProjectSearch/ParserTestResources/parserUnitTestsZip.zip");
            Assert.IsNotNull(zipArchive);
        }

        [TearDown]
        public void TearDown()
        {
            zipArchive.Dispose();
        }

        
        [Test()]
        public void MultipleAttributesEmpty()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("DocumentInformation/MultipleAttributesEmpty/RPN_AT_RDA_RDH{Dt_F_VND_V}_RD/Document_Information.tex");

            DocumentInformationExtractor die = new DocumentInformationExtractor(entry);

            DocumentInformation testObject = new DocumentInformation();
            testObject.ProjectReportName = null;
            testObject.AssetType = null;
            testObject.MainAuthor = "Lukas Nad";
            testObject.Authors = null;
            testObject.Reviewiers = new List<string> {"Katarina Amrichova"};
            testObject.Approvers = new List<string> { "Filip Mrocek" };
            testObject.ReportDocumentHistory = new List<ReportVersionEntry>();
            ReportVersionEntry report1 = new ReportVersionEntry();
            report1.VersionDate = new DateTime(2023, 6, 7);
            report1.Version = "0.2";
            report1.WholeName = "Michal Olencin";
            report1.ReportStatus = "Added Findings";
            testObject.ReportDocumentHistory.Add(report1);
            ReportVersionEntry report2 = new ReportVersionEntry();
            report2.VersionDate = new DateTime(2023, 6, 8);
            report2.Version = "0.4";
            report2.WholeName = "Katarina Amrichova";
            report2.ReportStatus = "Review";
            testObject.ReportDocumentHistory.Add(report2);


            DocumentInformation parsedObject = new DocumentInformation();
            parsedObject = die.ExtractDocumentInformation();

            Assert.IsNotNull(parsedObject);
            parsedObject.Should().BeEquivalentTo(testObject); 
            
            /*try
            {
             
            } catch(Exception ex)
            {
                Assert.Fail();
            }*/
        }

        [Test()]
        public void MutlipleAttributesMissing()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("DocumentInformation/MultipleAttributesMissing/RPN_RDA_RDMA_RDH_RV_RD_RDC/Document_Information.tex");

            DocumentInformationExtractor die = new DocumentInformationExtractor(entry);
            DocumentInformation parsedObject = new DocumentInformation();
            parsedObject = die.ExtractDocumentInformation();

            DocumentInformation testObject = new DocumentInformation();
            testObject.AssetType = "Mobile Application";
            testObject.Approvers = new List<string> { "Filip Mrocek" };
            testObject.Reviewiers = new List<string> { "Katarina Amrichova" };
            testObject.ReportDocumentHistory = new List<ReportVersionEntry>();

            Assert.IsNotNull(parsedObject);
            parsedObject.Should().BeEquivalentTo(testObject);

        }
        [Test()]
        public void fileEmpty()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("DocumentInformation/Empty/Document_Information.tex");

            DocumentInformationExtractor die = new DocumentInformationExtractor(entry);
            DocumentInformation parsedObject = new DocumentInformation();
            parsedObject = die.ExtractDocumentInformation();

            DocumentInformation testObject = new DocumentInformation();
            testObject.ReportDocumentHistory = new List<ReportVersionEntry>();

            parsedObject.Should().BeEquivalentTo(testObject);
        }

        [Test()]
        public void fullInformation()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("DocumentInformation/FullInformation/Document_Information.tex");

            DocumentInformationExtractor die = new DocumentInformationExtractor(entry);
            DocumentInformation parsedObject = new DocumentInformation();
            parsedObject = die.ExtractDocumentInformation();

            DocumentInformation testObject = new DocumentInformation();
            testObject.ProjectReportName = "Dummy Project 1";
            testObject.AssetType = "Mobile Application";
            testObject.MainAuthor = "Lukas Nad";
            testObject.Authors = new List<string> { 
                "Lukas Nad",
                "Taksh Medhavi",
                "Michal Olencin"
            };
            testObject.Reviewiers = new List<string> { "Katarina Amrichova" };
            testObject.Approvers = new List<string> { "Filip Mrocek" };
            testObject.ReportDocumentHistory = new List<ReportVersionEntry>();
            ReportVersionEntry report1 = new ReportVersionEntry();
            report1.VersionDate = new DateTime(2023, 6, 6);
            report1.Version = "0.1";
            report1.WholeName = "Lukas Nad";
            report1.ReportStatus = "Initial Draft";
            testObject.ReportDocumentHistory.Add(report1);
            ReportVersionEntry report2 = new ReportVersionEntry();
            report2.VersionDate = new DateTime(2023, 6, 7);
            report2.Version = "0.2";
            report2.WholeName = "Michal Olencin";
            report2.ReportStatus = "Added Findings";
            testObject.ReportDocumentHistory.Add(report2);
            ReportVersionEntry report3 = new ReportVersionEntry();
            report3.VersionDate = new DateTime(2023, 6, 8);
            report3.Version = "0.3";
            report3.WholeName = "Taksh Medhavi";
            report3.ReportStatus = "Added Findings";
            testObject.ReportDocumentHistory.Add(report3);
            ReportVersionEntry report4 = new ReportVersionEntry();
            report4.VersionDate = new DateTime(2023, 6, 8);
            report4.Version = "0.4";
            report4.WholeName = "Katarina Amrichova";
            report4.ReportStatus = "Review";
            testObject.ReportDocumentHistory.Add(report4);
            ReportVersionEntry report5 = new ReportVersionEntry();
            report5.VersionDate = new DateTime(2023, 6, 9);
            report5.Version = "0.5";
            report5.WholeName = "Lukas Nad";
            report5.ReportStatus = "Release";
            testObject.ReportDocumentHistory.Add(report5);

            testObject.ReportDate = new DateTime(2023, 6, 12);

            Assert.IsNotNull(parsedObject);
            parsedObject.Should().BeEquivalentTo(testObject);
        }
    }


}