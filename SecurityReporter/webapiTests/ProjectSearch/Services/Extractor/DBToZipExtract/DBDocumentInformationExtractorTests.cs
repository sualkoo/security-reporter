using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

[TestFixture]
public class DbDocumentInformationExtractorTests
{
    [Test]
    public void ExtractDocumentInformationTest()
    {
        // Arrange
        var documentInfo = new DocumentInformation
        {
            ProjectReportName = "Dummy Project 1",
            AssetType = "Mobile Application",
            MainAuthor = "Lukas Nad",
            Authors = new List<string>
            {
                "Lukas Nad"
            },
            Reviewiers = new List<string>
            {
                "Katarina Amrichova"
            },
            Approvers = new List<string>
            {
                "Filip Mrocek"
            }
        };

        var reportVersionEntry1 = new ReportVersionEntry();
        var reportVersionEntry2 = new ReportVersionEntry();

        reportVersionEntry1.VersionDate = new DateTime(2023, 08, 02);
        reportVersionEntry1.Version = "0.1";
        reportVersionEntry1.WholeName = "Lukas Nad";
        reportVersionEntry1.ReportStatus = "Initial Draft";

        reportVersionEntry2.VersionDate = new DateTime(2023, 08, 02);
        reportVersionEntry2.Version = "0.2";
        reportVersionEntry2.WholeName = "Michal Olencin";
        reportVersionEntry2.ReportStatus = "Added Findings";

        documentInfo.ReportDocumentHistory = new List<ReportVersionEntry>
        {
            reportVersionEntry1,
            reportVersionEntry2
        };

        documentInfo.ReportDate = new DateTime(2023, 6, 12);
        // Act
        var result = DbDocumentInformationExtractor.ExtractDocumentInformation(documentInfo);
        var resultDecoded = Encoding.UTF8.GetString(result);


        // Assert
        Assert.IsNotNull(result);
        StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tDOCUMENT TYPE\r\n%----------------------------------------------------------------------------------------\r\n\r\n% Use \"\\ReportDocument\" & \"\\TitlePageTableReport\" when writing Report \r\n% Use \"\\ScopeDocument\" & \"\\TitlePageTableScope\" for creating Scope Document\r\n\\newcommand{\\DocumentType}{\\ReportDocument}\r\n\\renewcommand{\\SetTitlePageTable}{\\TitlePageTableReport}\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tTITLE PAGE\r\n%----------------------------------------------------------------------------------------\r\n\\newcommand{\\ReportProjectName}{Dummy Project 1\\xspace}\r\n\\newcommand{\\ReportProjectType}{Penetration Test Report\\xspace}\r\n\\newcommand{\\AssetType}{Mobile Application\\xspace}\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tAUTHORS, REVIEWERS, APPROVERS\r\n%----------------------------------------------------------------------------------------\r\n\\newcommand{\\ReportDocumentMainAuthor}{Lukas Nad}\r\n\\newcommand{\\ReportDocumentAuthor}{Lukas Nad}\r\n\\newcommand{\\ReportDocumentReviewer}{Katarina Amrichova}\r\n\\newcommand{\\ReportDocumentApprover}{Filip Mrocek}\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tDOCUMENT VERSION HISTORY\r\n%----------------------------------------------------------------------------------------\r\n% Document version history. Copy the inner line for subsequent version entries.\r\n% Example: \\ReportVersionEntry{DATE}{VERSION}{FIRSTNAME LASTNAME}{draft / review / released}{COMMENT}\r\n\r\n% TODO: investigate git version tags (automatic compilation of document history table)\r\n\r\n\\newcommand{\\ReportDocumentHistory}{\r\n\t\\ReportVersionEntry{2023-08-02}{0.1}{Lukas Nad}{Initial Draft}\n\t\\ReportVersionEntry{2023-08-02}{0.2}{Michal Olencin}{Added Findings}\r\n}\r\n\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tGENERAL DOCUMENT INFORMATION\r\n%----------------------------------------------------------------------------------------\r\n\\newcommand{\\FiscalYear}{FY23}\r\n\\newcommand{\\ReportVersion}{Default}\r\n\\newcommand{\\ReportDate}{June 12, 2023}\r\n\\newcommand{\\ReportDocumentClassification}{CONFIDENTIAL}\r\n\r\n% \\newcommand{\\ReportStatus}{RELEASE} \r\n\\newcommand{\\ReportStatus}{DRAFT}", resultDecoded);
    }
}