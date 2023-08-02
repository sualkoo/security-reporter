using latexparse_csharp;
using System.Globalization;
using System.IO.Compression;
using System.Xml;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract
{
    public class DocumentInformationExtractor
    {
        private ZipArchiveEntry documentEntry;

        public DocumentInformationExtractor(ZipArchiveEntry documentEntry)
        {
            this.documentEntry = documentEntry;
        }

        public DocumentInformation ExtractDocumentInformation()
        {
            DocumentInformation newDocumentInfo = new DocumentInformation();
            newDocumentInfo.ReportDocumentHistory = new List<ReportVersionEntry>();
            if (documentEntry == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                using (StreamReader reader = new StreamReader(documentEntry.Open()))
                {
                    string line;
                    char[] delimiters = { '{', '}' };
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {

                            string trimmedLine = line.Trim();
                            if (trimmedLine[0] == '\\')
                            {
                                string[] inBracketContents = trimmedLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                if (inBracketContents[0] == "\\ReportVersionEntry" && inBracketContents.Length >= 5)
                                {
                                    newDocumentInfo.ReportDocumentHistory.Add(readReport(inBracketContents));
                                }
                                else if (inBracketContents.Length > 2)
                                {
                                    List<string> result = ReadInlineContents(inBracketContents[2]);
                                    assignNewData(inBracketContents[1], result, newDocumentInfo);
                                }
                            }
                        }
                    }
                }
                return newDocumentInfo;
            }
        }

        private List<string> ReadInlineContents(string extractedLine)
        {
            List<string> contents = null;
            string delimiter = "\\xspace";
            string[] cutString = extractedLine.Split(delimiter);
            if (cutString.Length > 0)
            {
                contents = new List<string>();
                string[] actualData = cutString[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (string data in actualData)
                {
                    contents.Add(data.Trim());
                }
            }

            return contents;
        }

        private ReportVersionEntry readReport(string[] inBracketContents)
        {
            ReportVersionEntry newReport = new ReportVersionEntry();
            try
            {
                newReport.VersionDate = DateTime.ParseExact(inBracketContents[1].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                Console.WriteLine("Incorrect ReportVersionEntry Version Date format, correct format" +
                    "is yyyy-MM-dd. " +
                    "Or incorrect format of ReportVersionEntry itself " +
                    "String found: " + inBracketContents[1].Trim());
            }
            newReport.Version = inBracketContents[2];
            newReport.WholeName = inBracketContents[3];
            newReport.ReportStatus = inBracketContents[4];

            return newReport;
        }

        private void assignNewData(string command, List<string> data, DocumentInformation newDocumentInfo)
        {
            if (data != null && data.Count > 0)
            {
                switch (command)
                {
                    case "\\ReportProjectName":
                        newDocumentInfo.ProjectReportName = data[0];
                        break;
                    case "\\AssetType":
                        newDocumentInfo.AssetType = data[0];
                        break;
                    case "\\ReportDocumentMainAuthor":
                        newDocumentInfo.MainAuthor = data[0];
                        break;
                    case "\\ReportDocumentAuthor":
                        newDocumentInfo.Authors = new List<string>();
                        foreach (string author in data)
                        {
                            newDocumentInfo.Authors.Add(author);
                        }
                        break;
                    case "\\ReportDocumentReviewer":
                        newDocumentInfo.Reviewiers = new List<string>();
                        foreach (string reviewer in data)
                        {
                            newDocumentInfo.Reviewiers.Add(reviewer);
                        }
                        break;
                    case "\\ReportDocumentApprover":
                        newDocumentInfo.Approvers = new List<string>();
                        foreach (string approver in data)
                        {
                            newDocumentInfo.Approvers.Add(approver);
                        }
                        break;
                    case "\\ReportDate":
                        try
                        {
                            newDocumentInfo.ReportDate = DateTime.ParseExact(data[0] + " " + data[1], "MMMM d yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine("Incorrect format of ReportDate, the correct format is MMMM d yyyy");
                        }
                        break;
                }
            }
        }
    }
}
