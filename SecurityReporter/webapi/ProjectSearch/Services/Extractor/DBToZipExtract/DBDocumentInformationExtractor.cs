using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DBDocumentInformationExtractor
    {
        public byte[] extractDocumentInformation(DocumentInformation documentInformation)
        {
            if (documentInformation == null)
            {
                throw new ArgumentNullException(nameof(documentInformation), "Document information cannot be null.");
            }

            // Validate required properties before processing
            if (string.IsNullOrEmpty(documentInformation.ProjectReportName))
            {
                throw new ArgumentNullException(nameof(documentInformation.ProjectReportName), "Project report name cannot be null or empty.");
            }
            if (documentInformation.Authors == null)
            {
                documentInformation.Authors = new List<string>();
            }
            if (documentInformation.Approvers == null)
            {
                documentInformation.Approvers = new List<string>();
            }
            if (documentInformation.Reviewiers == null)
            {
                documentInformation.Reviewiers = new List<string>();
            }

            if (documentInformation.ReportDocumentHistory == null)
            {
                documentInformation.ReportDocumentHistory = new List<ReportVersionEntry>(); // Initialize to an empty list if null
            }

            // Rest of the null checks can be added for other properties as needed...

            List<string> reportVersionEntries = new List<string>();
            foreach (ReportVersionEntry rve in documentInformation.ReportDocumentHistory)
            {
                // Validate ReportVersionEntry properties as needed...

                string entryString = "\t\\ReportVersionEntry{" + rve.VersionDate.ToString("yyyy-MM-dd") + "}{" + rve.Version + "}{" + rve.WholeName + "}{" + rve.ReportStatus + "}";
                reportVersionEntries.Add(entryString);
            }
            string documentInfoContent = 
@"%----------------------------------------------------------------------------------------
%	DOCUMENT TYPE
%----------------------------------------------------------------------------------------

% Use ""\ReportDocument"" & ""\TitlePageTableReport"" when writing Report 
% Use ""\ScopeDocument"" & ""\TitlePageTableScope"" for creating Scope Document
\newcommand{\DocumentType}{\ReportDocument}
\renewcommand{\SetTitlePageTable}{\TitlePageTableReport}

%----------------------------------------------------------------------------------------
%	TITLE PAGE
%----------------------------------------------------------------------------------------
\newcommand{\ReportProjectName}{" + documentInformation.ProjectReportName + @"\xspace}
\newcommand{\ReportProjectType}{Penetration Test Report\xspace}
\newcommand{\AssetType}{" + documentInformation.AssetType + @"\xspace}

%----------------------------------------------------------------------------------------
%	AUTHORS, REVIEWERS, APPROVERS
%----------------------------------------------------------------------------------------
\newcommand{\ReportDocumentMainAuthor}{" + documentInformation.MainAuthor + @"}
\newcommand{\ReportDocumentAuthor}{" + string.Join(", ", documentInformation.Authors) + @"}
\newcommand{\ReportDocumentReviewer}{" + string.Join(", ", documentInformation.Reviewiers) + @"}
\newcommand{\ReportDocumentApprover}{" + string.Join(", ", documentInformation.Approvers) + @"}

%----------------------------------------------------------------------------------------
%	DOCUMENT VERSION HISTORY
%----------------------------------------------------------------------------------------
% Document version history. Copy the inner line for subsequent version entries.
% Example: \ReportVersionEntry{DATE}{VERSION}{FIRSTNAME LASTNAME}{draft / review / released}{COMMENT}

% TODO: investigate git version tags (automatic compilation of document history table)

\newcommand{\ReportDocumentHistory}{
" + string.Join("\n", reportVersionEntries) + @"
}


%----------------------------------------------------------------------------------------
%	GENERAL DOCUMENT INFORMATION
%----------------------------------------------------------------------------------------
\newcommand{\FiscalYear}{FY23}
\newcommand{\ReportVersion}{Default}
\newcommand{\ReportDate}{\today}
\newcommand{\ReportDocumentClassification}{CONFIDENTIAL}

% \newcommand{\ReportStatus}{RELEASE} 
\newcommand{\ReportStatus}{DRAFT}";

            return Encoding.UTF8.GetBytes(documentInfoContent);
        }
    }
}
