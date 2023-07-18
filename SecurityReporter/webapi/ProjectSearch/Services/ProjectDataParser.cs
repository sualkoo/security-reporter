using System;
using System.Globalization;
using System.IO.Compression;
using System.IO.Pipes;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;

namespace webapi.ProjectSearch.Services
{
    public class ProjectDataParser : IProjectDataParser
    {
        public ProjectReportData Extract(Stream zipStream)
        {
            ProjectReportData newProjectReportData  = new ProjectReportData();
            
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read)) {
                if(zipStream != null) {
                    ZipArchiveEntry currentEntry = archive.GetEntry("Config/Document_Information.tex");
                    newProjectReportData.DocumentInfo = ExtractDocumentInformation(currentEntry);
                    currentEntry = archive.GetEntry("Config/Executive_Summary.tex");
                    newProjectReportData.ExecutiveSummary = ExtractExecutiveSummary(currentEntry);
                    currentEntry = archive.GetEntry("Config/Project_Information.tex");
                    ProjectInformationExtractor pie = new ProjectInformationExtractor(currentEntry);
                    newProjectReportData.ProjectInfo = pie.ExtractProjectInformation();
                    currentEntry = archive.GetEntry("Config/Testing_Methodology.tex");
                    TestingMethodologyExtractor tme = new TestingMethodologyExtractor(currentEntry);
                    newProjectReportData.TestingMethodology = tme.ExtractTestingMethodology();

                }
                else
                {
                    throw new ArgumentNullException(nameof(zipStream));
                }
            }
            return newProjectReportData;
        }

        private DocumentInformation ExtractDocumentInformation(ZipArchiveEntry documentEntry)
        {
            DocumentInformation newDocumentInfo = new DocumentInformation();
            newDocumentInfo.ReportDocumentHistory = new List<ReportVersionEntry>();
            if (documentEntry == null) {
                throw new ArgumentNullException();
            } 
            else
            {
                using(StreamReader reader = new StreamReader(documentEntry.Open()))
                {

                    string line;
                    char[] delimiters = { '{', '}', '\\' };
                    while ((line = reader.ReadLine()) != null)
                    {
                        
                        if (!string.IsNullOrEmpty(line) && (line[0] == '\\' || line[0] == '\t'))
                        {
                            string[] inBracketContents = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (inBracketContents[1] == "ReportVersionEntry")
                            {
                                ReportVersionEntry newReport = new ReportVersionEntry();
                                DateTime newReportVersionDate = DateTime.ParseExact(inBracketContents[2].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture); ;
                                newReport.VersionDate = new DateOnly(newReportVersionDate.Year, newReportVersionDate.Month, newReportVersionDate.Day);
                                newReport.Version = inBracketContents[3];
                                newReport.WholeName = inBracketContents[4];
                                newReport.ReportStatus = inBracketContents[5];
                                newDocumentInfo.ReportDocumentHistory.Add(newReport);
                            } else
                            {
                                if(inBracketContents.Length > 2)
                                {
                                    List<string> result = ReadInlineContents(inBracketContents[2]);
                                    assignNewData(inBracketContents[1], result, newDocumentInfo);
                                }
                            }
                        }

                        //Console.WriteLine();
                    }
                }
                return newDocumentInfo;
            }
        }

        private string ExtractExecutiveSummary(ZipArchiveEntry execSumEntry)
        {
            DocumentInformation newDocumentInfo = new DocumentInformation();
            newDocumentInfo.ReportDocumentHistory = new List<ReportVersionEntry>();
            string line;
            bool readingExecSum = false;
            string resultString = "";

            if (execSumEntry == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                using (StreamReader reader = new StreamReader(execSumEntry.Open()))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line == "%-<ExecSum>")
                            {
                                readingExecSum = false;
                            }

                            if (readingExecSum)
                            {
                                resultString += line;
                            }

                            if (line == "%-<ExecSum>->")
                            {
                                readingExecSum = true;
                            }
                            
                        }

                    }
                }
                return resultString;
            }
        }

        /*Parameter is a string with data in it, so it's the part of a string, that is in second curly braces.
         It takes whole line inside the braces with data, then splits it according to commas and trims the whitespaces. */
        private List<string> ReadInlineContents(string extractedLine)
        {
            char[] delimiters = { '}', '\\' };
            string[] cutString = extractedLine.Split(delimiters);
            string[] actualData = cutString[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<string> contents = new List<string>();
            foreach (string data in actualData)
            {
                contents.Add(data.Trim());
            }

            return contents;
        }


        private void assignNewData(string command, List<string> data, DocumentInformation newDocumentInfo)
        {
            switch (command)
            {
                case "ReportProjectName":
                    newDocumentInfo.ProjectReportName = data[0];
                    break;
                case "AssetType":
                    newDocumentInfo.AssetType = data[0];
                    break;
                case "ReportDocumentMainAuthor":
                    newDocumentInfo.MainAuthor = data[0];
                    break;
                case "ReportDocumentAuthor":
                    newDocumentInfo.Authors = new List<string>();
                    foreach(string author in data)
                    {
                        newDocumentInfo.Authors.Add(author);
                    }
                    break;
                case "ReportDocumentReviewer":
                    newDocumentInfo.Reviewiers = new List<string>();
                    foreach (string reviewer in data)
                    {
                        newDocumentInfo.Reviewiers.Add(reviewer);
                    }
                    break;
                case "ReportDocumentApprover":
                    newDocumentInfo.Approvers = new List<string>();
                    foreach (string approver in data)
                    {
                        newDocumentInfo.Approvers.Add(approver);
                    }
                    break;
                case "ReportDate":
                    //ZMENIT ATRIBUT NA STRING
                    DateTime newReportDate;
                    newReportDate = DateTime.ParseExact(data[0] + " " + data[1], "MMMM d yyyy", CultureInfo.InvariantCulture);
                    newDocumentInfo.ReportDate = new DateOnly(newReportDate.Year, newReportDate.Month, newReportDate.Day);
                    break;
            }
        }
    }
}
