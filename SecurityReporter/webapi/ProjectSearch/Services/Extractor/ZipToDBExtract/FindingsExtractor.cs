﻿using System.IO.Compression;
using System.Text.RegularExpressions;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract
{
    public class FindingsExtractor
    {
        string newCommandRegexPattern = @"\\(?:renew|new)command{\\([^}]+)}{([^}]+)}";
        string subsectionRegex = @"\\subsection\*{([^}]+)}([\s\S]*?)(?=\\subsection\*{[^}]+}|\\subsection\*|$)";


        public Finding ExtractFinding(ZipArchiveEntry currentEntry)
        {
            Finding newFinding = new Finding();
            string line;
            char[] delimiters = { '{', '}' };
            using (StreamReader reader = new StreamReader(currentEntry.Open()))
            {
                string fileContent = reader.ReadToEnd();
                MatchCollection newCommandMatches = Regex.Matches(fileContent, newCommandRegexPattern);
                MatchCollection subsectionMatches = Regex.Matches(fileContent, subsectionRegex);
                foreach (Match match in newCommandMatches)
                {
                    assignNewData(match.Groups[1].ToString(), match.Groups[2].ToString(), newFinding);
                }
                foreach (Match match in subsectionMatches)
                {
                    assignNewData(match.Groups[1].ToString(), match.Groups[2].ToString(), newFinding);
                }
                /*while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line) && line.Trim().Length > 0 && line.Trim()[0] == '\\')
                    {
                        string[] splitString = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        if (splitString.Length == 3)
                        {
                            assignNewData(splitString[1], splitString[2], newFinding);
                        }
                        else if (splitString.Length == 2)
                        {
                            if (splitString[0] == "\\subsection*")
                            {
                                extractSubsection(splitString[1], reader, newFinding);
                            }
                        }
                    }
                }*/

                return newFinding;
            }
        }

        private void assignNewData(string command, string data, Finding newFinding)
        {
            switch (command)
            {
                case "FindingAuthor":
                    newFinding.FindingAuthor = data;
                    break;
                case "FindingName":
                    newFinding.FindingName = data;
                    break;
                case "Location":
                    newFinding.Location = extractLocation(data);
                    break;
                case "Component":
                    newFinding.Component = data;
                    break;
                case "FoundWith":
                    newFinding.FoundWith = data;
                    break;
                case "TestMethod":
                    newFinding.TestMethod = data;
                    break;
                case "CVSS":
                    newFinding.CVSS = data;
                    break;
                case "CVSSvector":
                    newFinding.CVSSVector = data;
                    break;
                case "CWE":
                    newFinding.CWE = int.Parse(data);
                    break;
                case "Criticality":
                    switch (data)
                    {
                        case "High":
                            newFinding.Criticality = Enums.Criticality.High;
                            break;
                        case "Medium":
                            newFinding.Criticality = Enums.Criticality.Medium;
                            break;
                        case "Low":
                            newFinding.Criticality = Enums.Criticality.Low;
                            break;
                        case "Info":
                            newFinding.Criticality = Enums.Criticality.Info;
                            break;
                        case "To Be Rated":
                            newFinding.Criticality = Enums.Criticality.TBR;
                            break;
                    }
                    break;
                case "Exploitability":
                    switch (data)
                    {
                        case "Easy":
                            newFinding.Exploitability = Enums.Exploitability.Easy;
                            break;
                        case "Average":
                            newFinding.Exploitability = Enums.Exploitability.Average;
                            break;
                        case "Hard":
                            newFinding.Exploitability = Enums.Exploitability.Hard;
                            break;
                        case "To Be Rated":
                            newFinding.Exploitability = Enums.Exploitability.TBR;
                            break;
                    }
                    break;
                case "Category":
                    switch (data)
                    {
                        case "Access Control":
                            newFinding.Category = Enums.Category.AccessControl;
                            break;
                        case "Application Design":
                            newFinding.Category = Enums.Category.ApplicationDesign;
                            break;
                        case "Information Disclosure":
                            newFinding.Category = Enums.Category.InformationDisclosure;
                            break;
                        case "Outdated Software":
                            newFinding.Category = Enums.Category.OutdatedSoftware;
                            break;
                        case "Security Configuration":
                            newFinding.Category = Enums.Category.SecurityConfiguration;
                            break;
                    }
                    break;
                case "Detectability":
                    switch (data)
                    {
                        case "Easy":
                            newFinding.Detectability = Enums.Detectability.Easy;
                            break;
                        case "Average":
                            newFinding.Detectability = Enums.Detectability.Average;
                            break;
                        case "Difficult":
                            newFinding.Detectability = Enums.Detectability.Difficult;
                            break;
                        case "To Be Rated":
                            newFinding.Detectability = Enums.Detectability.TBR;
                            break;
                    }
                    break;
                case "Details":
                    newFinding.UnformattedSubsectionDetails = data;
                    newFinding.SubsectionDetails =  extractSubsection(data);
                    break;
                case "Impact":
                    newFinding.UnformattedSubsectionImpact = data;
                    newFinding.SubsectionImpact = extractSubsection(data);
                    break;
                case "Repeatability":
                    newFinding.UnformattedSubsectionRepeatability = data;
                    newFinding.SubsectionRepeatability = extractSubsection(data);
                    break;
                case "Countermeasures":
                    newFinding.UnformattedSubsectionCountermeasures = data;
                    newFinding.SubsectionCountermeasures = extractSubsection(data);
                    break;
                case "References":
                    newFinding.UnformattedSubsectionReferences = data;
                    string referencesRegex = @"\\href{([^}]*)}";
                    MatchCollection matches = Regex.Matches(data, referencesRegex, RegexOptions.Multiline);
                    string result = "";
                    foreach(Match match in matches)
                    {
                        result += (match.Groups[1].ToString() + "\n");
                    }

                    newFinding.SubsectionReferences = "This finding references the following information sources: \n" + result;
                    break;
            }
        }

        private static List<string> extractLocation(string data)
        {
            var result = data.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            for (var i = 0; i < result.Count; i++) result[i] = result[i].Trim();

            return result;
        }

        private string extractSubsection(string data)
        {
            string result = data;
            string regexCommandsBeggining = @"\\[\w]*{";
            string regexComments = @"^(%\s*.*)";
            string regexFigures = @"\\begin{figure}[\w\W]*?\\end{figure}";
            
            result = Regex.Replace(result, regexFigures, "", RegexOptions.Multiline);
            result = Regex.Replace(result, regexCommandsBeggining, "", RegexOptions.Multiline);
            result = Regex.Replace(result, regexComments, "", RegexOptions.Multiline);

            result = result.Replace("}", "");
            result = result.Replace("\n", " ");
            result = result.Replace("\r", "");
            result = result.Trim();
            return result;
            //result = result.Split("")
        }

        /*private void extractSubsection(string command, StreamReader reader, Finding newFinding)
        {
            string line;
            bool reading = true;
            string result = "";
            while ((line = reader.ReadLine()) != null && reading)
            {

                string trimmedLine = line.Trim();
                //reading = trimmedLine.Length >= 6 && trimmedLine.Substring(0, 6) == "\\begin" ? false : true;
                reading = trimmedLine.Length >= 11 && trimmedLine.Substring(0, 11) == "\\subsection" ? false : true;
                //reading = trimmedLine.Length > 0 && trimmedLine[0] == '%' ? false : true;

                if (reading)
                {
                    result += (line + '\n');
                }
                else
                {
                    assignNewData(command, result, newFinding);
                    if (trimmedLine.Length >= 12 && trimmedLine.Substring(0, 11) == "\\subsection")
                    {
                        char[] delimiters = { '{', '}' };
                        string[] splitString = trimmedLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        if (splitString.Length == 2)
                        {
                            extractSubsection(splitString[1], reader, newFinding);
                        }
                    }
                }
                
            }
            if(line == null)
            {
                assignNewData(command, result, newFinding);
            }
        }*/

        /*private List<string> extractReferences(string data, StreamReader reader, Finding newFinding)
        {
            string line;
            string resultList = "";
            string currentItem = "";
            bool firstItem = true;

            while ((line = reader.ReadLine()) != null)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.Length >= 13)
                {
                    if (trimmedLine.Substring(0, 13) == "\\end{itemize}")
                    {
                        if (currentItem != "")
                        {
                            resultList = resultList + currentItem.Trim();
                        }
                        return resultList;
                    }
                }

                if (trimmedLine.Length >= 5 && trimmedLine.Substring(0, 5) == "\\item")
                {
                    if (!firstItem)
                    {
                        resultList = resultList + currentItem.Trim();
                        currentItem = "";
                    }

                    currentItem += trimmedLine.Substring(5);
                    firstItem = false;
                }
                else if (!string.IsNullOrEmpty(trimmedLine) && trimmedLine.Length > 0 && !firstItem)
                {
                    currentItem += trimmedLine;
                }
            }

            return resultList;
        }*/
    }

}