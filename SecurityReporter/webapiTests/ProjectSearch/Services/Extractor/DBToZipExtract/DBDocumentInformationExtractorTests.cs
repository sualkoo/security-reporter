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
        StringAssert.Contains("%----------------------------------------------------------------------------------------\n%\tDOCUMENT TYPE\n%----------------------------------------------------------------------------------------\n\n% Use \"\\ReportDocument\" & \"\\TitlePageTableReport\" when writing Report \n% Use \"\\ScopeDocument\" & \"\\TitlePageTableScope\" for creating Scope Document\n\\newcommand{\\DocumentType}{\\ReportDocument}\n\\renewcommand{\\SetTitlePageTable}{\\TitlePageTableReport}\n\n%----------------------------------------------------------------------------------------\n%\tTITLE PAGE\n%----------------------------------------------------------------------------------------\n\\newcommand{\\ReportProjectName}{Dummy Project 1\\xspace}\n\\newcommand{\\ReportProjectType}{Penetration Test Report\\xspace}\n\\newcommand{\\AssetType}{Mobile Application\\xspace}\n\n%----------------------------------------------------------------------------------------\n%\tAUTHORS, REVIEWERS, APPROVERS\n%----------------------------------------------------------------------------------------\n\\newcommand{\\ReportDocumentMainAuthor}{Lukas Nad}\n\\newcommand{\\ReportDocumentAuthor}{Lukas Nad}\n\\newcommand{\\ReportDocumentReviewer}{Katarina Amrichova}\n\\newcommand{\\ReportDocumentApprover}{Filip Mrocek}\n\n%----------------------------------------------------------------------------------------\n%\tDOCUMENT VERSION HISTORY\n%----------------------------------------------------------------------------------------\n% Document version history. Copy the inner line for subsequent version entries.\n% Example: \\ReportVersionEntry{DATE}{VERSION}{FIRSTNAME LASTNAME}{draft / review / released}{COMMENT}\n\n% TODO: investigate git version tags (automatic compilation of document history table)\n\n\\newcommand{\\ReportDocumentHistory}{\n\t\\ReportVersionEntry{2023-08-02}{0.1}{Lukas Nad}{Initial Draft}\n\t\\ReportVersionEntry{2023-08-02}{0.2}{Michal Olencin}{Added Findings}\n}\n\n\n%----------------------------------------------------------------------------------------\n%\tGENERAL DOCUMENT INFORMATION\n%----------------------------------------------------------------------------------------\n\\newcommand{\\FiscalYear}{FY23}\n\\newcommand{\\ReportVersion}{Default}\n\\newcommand{\\ReportDate}{June 12, 2023}\n\\newcommand{\\ReportDocumentClassification}{CONFIDENTIAL}\n\n% \\newcommand{\\ReportStatus}{RELEASE} \n\\newcommand{\\ReportStatus}{DRAFT}", resultDecoded);
    }
}