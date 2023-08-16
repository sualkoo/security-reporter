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

        string expectedStr = @"%----------------------------------------------------------------------------------------
%	DOCUMENT TYPE
%----------------------------------------------------------------------------------------

% Use ""\ReportDocument"" & ""\TitlePageTableReport"" when writing Report 
% Use ""\ScopeDocument"" & ""\TitlePageTableScope"" for creating Scope Document
\newcommand{\DocumentType}{\ReportDocument}
\renewcommand{\SetTitlePageTable}{\TitlePageTableReport}

%----------------------------------------------------------------------------------------
%	TITLE PAGE
%----------------------------------------------------------------------------------------
\newcommand{\ReportProjectName}{Dummy Project 1\xspace}
\newcommand{\ReportProjectType}{Penetration Test Report\xspace}
\newcommand{\AssetType}{Mobile Application\xspace}

%----------------------------------------------------------------------------------------
%	AUTHORS, REVIEWERS, APPROVERS
%----------------------------------------------------------------------------------------
\newcommand{\ReportDocumentMainAuthor}{Lukas Nad}
\newcommand{\ReportDocumentAuthor}{Lukas Nad}
\newcommand{\ReportDocumentReviewer}{Katarina Amrichova}
\newcommand{\ReportDocumentApprover}{Filip Mrocek}

%----------------------------------------------------------------------------------------
%	DOCUMENT VERSION HISTORY
%----------------------------------------------------------------------------------------
% Document version history. Copy the inner line for subsequent version entries.
% Example: \ReportVersionEntry{DATE}{VERSION}{FIRSTNAME LASTNAME}{draft / review / released}{COMMENT}

% TODO: investigate git version tags (automatic compilation of document history table)

\newcommand{\ReportDocumentHistory}{
	\ReportVersionEntry{2023-08-02}{0.1}{Lukas Nad}{Initial Draft}
	\ReportVersionEntry{2023-08-02}{0.2}{Michal Olencin}{Added Findings}
}


%----------------------------------------------------------------------------------------
%	GENERAL DOCUMENT INFORMATION
%----------------------------------------------------------------------------------------
\newcommand{\FiscalYear}{FY23}
\newcommand{\ReportVersion}{Default}
\newcommand{\ReportDate}{June 12, 2023}
\newcommand{\ReportDocumentClassification}{CONFIDENTIAL}

% \newcommand{\ReportStatus}{RELEASE} 
\newcommand{\ReportStatus}{DRAFT}";
        
        // Act
        var result = DbDocumentInformationExtractor.ExtractDocumentInformation(documentInfo);
        var resultDecoded = Encoding.UTF8.GetString(result);


        // Assert
        Assert.IsNotNull(result);
        StringAssert.Contains(expectedStr, resultDecoded);
    }
}