using System;
using System.Collections.Generic;
using System.IO.Compression;
using FluentAssertions;
using NUnit.Framework;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

namespace webapiTests.ProjectSearch.Services;

[TestFixture]
public class DocumentInformationExtractorTests
{
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

    private ZipArchive zipArchive;


    [Test]
    public void MultipleAttributesEmpty()
    {
        var entry = zipArchive.GetEntry(
            "DocumentInformation/MultipleAttributesEmpty/RPN_AT_RDA_RDH{Dt_F_VND_V}_RD/Document_Information.tex");

        var die = new DocumentInformationExtractor(entry);

        var testObject = new DocumentInformation
        {
            ProjectReportName = null,
            AssetType = null,
            MainAuthor = "Lukas Nad",
            Authors = null,
            Reviewiers = new List<string> { "Katarina Amrichova" },
            Approvers = new List<string> { "Filip Mrocek" },
            ReportDocumentHistory = new List<ReportVersionEntry>()
        };
        var report1 = new ReportVersionEntry
        {
            VersionDate = new DateTime(2023, 6, 7),
            Version = "0.2",
            WholeName = "Michal Olencin",
            ReportStatus = "Added Findings"
        };
        testObject.ReportDocumentHistory.Add(report1);
        var report2 = new ReportVersionEntry
        {
            VersionDate = new DateTime(2023, 6, 8),
            Version = "0.4",
            WholeName = "Katarina Amrichova",
            ReportStatus = "Review"
        };
        testObject.ReportDocumentHistory.Add(report2);
        var report3 = new ReportVersionEntry
        {
            VersionDate = new DateTime(2023, 6, 8),
            Version = "",
            WholeName = "",
            ReportStatus = ""
        };
        testObject.ReportDocumentHistory.Add(report3);
        var report4 = new ReportVersionEntry
        {
            ReportStatus = "Release",
            VersionDate = new DateTime(2023, 6, 9),
            WholeName = "Lukas Nad",
            Version = ""
        };
        testObject.ReportDocumentHistory.Add(report4);


        var parsedObject = new DocumentInformation();
        parsedObject = die.ExtractDocumentInformation();

        Assert.IsNotNull(parsedObject);
        parsedObject.Should().BeEquivalentTo(testObject);
    }

    [Test]
    public void MutlipleAttributesMissing()
    {
        var entry = zipArchive.GetEntry(
            "DocumentInformation/MultipleAttributesMissing/RPN_RDA_RDMA_RDH_RV_RD_RDC/Document_Information.tex");

        var die = new DocumentInformationExtractor(entry);
        var parsedObject = die.ExtractDocumentInformation();

        var testObject = new DocumentInformation
        {
            AssetType = "Mobile Application",
            Approvers = new List<string> { "Filip Mrocek" },
            Reviewiers = new List<string> { "Katarina Amrichova" },
            ReportDocumentHistory = new List<ReportVersionEntry>()
        };

        Assert.IsNotNull(parsedObject);
        parsedObject.Should().BeEquivalentTo(testObject);
    }

    [Test]
    public void FileEmpty()
    {
        var entry = zipArchive.GetEntry("DocumentInformation/Empty/Document_Information.tex");

        var die = new DocumentInformationExtractor(entry);
        var parsedObject = die.ExtractDocumentInformation();

        var testObject = new DocumentInformation
        {
            ReportDocumentHistory = new List<ReportVersionEntry>()
        };

        parsedObject.Should().BeEquivalentTo(testObject);
    }

    [Test]
    public void FullInformation()
    {
        var entry = zipArchive.GetEntry("DocumentInformation/FullInformation/Document_Information.tex");

        var die = new DocumentInformationExtractor(entry);
        var parsedObject = die.ExtractDocumentInformation();

        var testObject = new DocumentInformation
        {
            ProjectReportName = "Dummy Project 1",
            AssetType = "Mobile Application",
            MainAuthor = "Lukas Nad",
            Authors = new List<string>
            {
                "Lukas Nad",
                "Taksh Medhavi",
                "Michal Olencin"
            },
            Reviewiers = new List<string> { "Katarina Amrichova" },
            Approvers = new List<string> { "Filip Mrocek" },
            ReportDocumentHistory = new List<ReportVersionEntry>()
        };
        var report1 = new ReportVersionEntry
        {
            VersionDate = new DateTime(2023, 6, 6),
            Version = "0.1",
            WholeName = "Lukas Nad",
            ReportStatus = "Initial Draft"
        };
        testObject.ReportDocumentHistory.Add(report1);
        var report2 = new ReportVersionEntry
        {
            VersionDate = new DateTime(2023, 6, 7),
            Version = "0.2",
            WholeName = "Michal Olencin",
            ReportStatus = "Added Findings"
        };
        testObject.ReportDocumentHistory.Add(report2);
        var report3 = new ReportVersionEntry
        {
            VersionDate = new DateTime(2023, 6, 8),
            Version = "0.3",
            WholeName = "Taksh Medhavi",
            ReportStatus = "Added Findings"
        };
        testObject.ReportDocumentHistory.Add(report3);
        var report4 = new ReportVersionEntry
        {
            VersionDate = new DateTime(2023, 6, 8),
            Version = "0.4",
            WholeName = "Katarina Amrichova",
            ReportStatus = "Review"
        };
        testObject.ReportDocumentHistory.Add(report4);
        var report5 = new ReportVersionEntry
        {
            VersionDate = new DateTime(2023, 6, 9),
            Version = "0.5",
            WholeName = "Lukas Nad",
            ReportStatus = "Release"
        };
        testObject.ReportDocumentHistory.Add(report5);

        testObject.ReportDate = new DateTime(2023, 6, 12);

        Assert.IsNotNull(parsedObject);
        parsedObject.Should().BeEquivalentTo(testObject);
    }
}