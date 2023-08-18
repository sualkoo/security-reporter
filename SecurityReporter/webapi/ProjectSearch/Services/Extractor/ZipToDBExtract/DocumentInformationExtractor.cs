using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

public class DocumentInformationExtractor
{
    private readonly ZipArchiveEntry documentEntry;

    public DocumentInformationExtractor(ZipArchiveEntry documentEntry)
    {
        this.documentEntry = documentEntry;
    }

    public DocumentInformation ExtractDocumentInformation()
    {
        var newDocumentInfo = new DocumentInformation
        {
            ReportDocumentHistory = new List<ReportVersionEntry>()
        };
        if (documentEntry == null)
            throw new ArgumentNullException();
        using (var reader = new StreamReader(documentEntry.Open()))
        {
            var fileContent = reader.ReadToEnd();
            var regexPattern =
                @"\\(newcommand|renewcommand)\s*\{\\([a-zA-Z]*)\}\s*\{((?>[^{}]+|\{(?<DEPTH>)|\}(?<-DEPTH>))*(?(DEPTH)(?!)))\}";

            var matches = Regex.Matches(fileContent, regexPattern);
            string line;
            char[] delimiters = { '{', '}' };

            foreach (Match match in matches)
                if (match.Groups[2].Value == "ReportDocumentHistory")
                {
                    var reportRegex = @"\\ReportVersionEntry{(\d{4}-\d{2}-\d{2})}{(.*?)}{([\w\s]*?)}{([\w\s]*?)}";
                    var reportVersionMatches = Regex.Matches(fileContent, reportRegex);
                    foreach (Match reportMatch in reportVersionMatches)
                    {
                        var inBracketContents = new string[5];
                        for (var i = 0; i < reportMatch.Groups.Count; i++)
                            inBracketContents[i] = reportMatch.Groups[i].Value;
                        newDocumentInfo.ReportDocumentHistory.Add(ReadReport(inBracketContents));
                    }
                }
                else
                {
                    var result = ReadInlineContents(match.Groups[3].Value);
                    AssignNewData(match.Groups[2].Value, result, newDocumentInfo);
                }

            /*while ((line = reader.ReadLine()) != null)
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
            }*/
            return newDocumentInfo;
        }
    }

    private static List<string> ReadInlineContents(string extractedLine)
    {
        List<string> contents = null;
        var delimiter = "\\xspace";
        var cutString = extractedLine.Split(delimiter);
        if (cutString.Length > 0)
        {
            contents = new List<string>();
            var actualData = cutString[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var data in actualData) contents.Add(data.Trim());
        }

        return contents;
    }

    private static ReportVersionEntry ReadReport(string[] inBracketContents)
    {
        var newReport = new ReportVersionEntry();
        try
        {
            newReport.VersionDate =
                DateTime.ParseExact(inBracketContents[1].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        catch (FormatException ex)
        {
            throw new CustomException(StatusCodes.Status400BadRequest,
                "Incorrect ReportVersionEntry Version Date format, correct format" +
                "is yyyy-MM-dd.");
        }

        newReport.Version = inBracketContents[2];
        newReport.WholeName = inBracketContents[3];
        newReport.ReportStatus = inBracketContents[4];

        return newReport;
    }

    private static void AssignNewData(string command, List<string> data, DocumentInformation newDocumentInfo)
    {
        if (data != null && data.Count > 0)
            switch ('\\' + command)
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
                    foreach (var author in data) newDocumentInfo.Authors.Add(author);
                    break;
                case "\\ReportDocumentReviewer":
                    newDocumentInfo.Reviewiers = new List<string>();
                    foreach (var reviewer in data) newDocumentInfo.Reviewiers.Add(reviewer);
                    break;
                case "\\ReportDocumentApprover":
                    newDocumentInfo.Approvers = new List<string>();
                    foreach (var approver in data) newDocumentInfo.Approvers.Add(approver);
                    break;
                case "\\ReportDate":
                    try
                    {
                        newDocumentInfo.ReportDate = DateTime.ParseExact(data[0] + " " + data[1], "MMMM d yyyy",
                            CultureInfo.InvariantCulture);
                    }
                    catch (FormatException ex)
                    {
                        throw new CustomException(StatusCodes.Status400BadRequest,
                            "Incorrect format of ReportDate, the correct format is MMMM d yyyy");
                    }

                    break;
            }
    }
}