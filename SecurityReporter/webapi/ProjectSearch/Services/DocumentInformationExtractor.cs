using System.Globalization;
using System.IO.Compression;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services
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
                    char[] delimiters = { '{', '}'};
                    while ((line = reader.ReadLine()) != null)
                    {

                        if (!string.IsNullOrEmpty(line) && line.Length > 0)
                        {
                            string trimmedLine = line.Trim();
                            if(trimmedLine[0] == '\\')
                            {
                                string[] inBracketContents = trimmedLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                if (inBracketContents[0] == "\\ReportVersionEntry")
                                {
                                    ReportVersionEntry newReport = new ReportVersionEntry();
                                    newReport.VersionDate = DateTime.ParseExact(inBracketContents[1].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    newReport.Version = inBracketContents[2];
                                    newReport.WholeName = inBracketContents[3];
                                    newReport.ReportStatus = inBracketContents[4];
                                    newDocumentInfo.ReportDocumentHistory.Add(newReport);
                                }
                                else
                                {
                                    if (inBracketContents.Length > 2)
                                    {
                                        List<string> result = ReadInlineContents(inBracketContents[2]);
                                        assignNewData(inBracketContents[1], result, newDocumentInfo);
                                    }
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
            char delimiter = '\\';
            string[] cutString = extractedLine.Split(delimiter);
            if(cutString.Length > 0 && (cutString[0].Trim() != "xspace")) 
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

        private void assignNewData(string command, List<string> data, DocumentInformation newDocumentInfo)
        {
            if(data != null)
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
                        //ZMENIT ATRIBUT NA STRING
                        newDocumentInfo.ReportDate = DateTime.ParseExact(data[0] + " " + data[1], "MMMM d yyyy", CultureInfo.InvariantCulture);
                        break;
                }
            }
        }
    }
}
