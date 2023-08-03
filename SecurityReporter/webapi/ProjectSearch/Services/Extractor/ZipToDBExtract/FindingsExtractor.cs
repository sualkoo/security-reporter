using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Compression;
using webapi.Models;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract
{
    public class FindingsExtractor
    {
        List<Finding> findingsList = new List<Finding>();
        public Finding extractFinding(ZipArchiveEntry currentEntry)
        {
            Finding newFinding = new Finding();
            string line;
            char[] delimiters = { '{', '}' };
            using (StreamReader reader = new StreamReader(currentEntry.Open()))
            {
                while ((line = reader.ReadLine()) != null)
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
                }

                return newFinding;
            }
        }

        private void assignNewData(string command, string data, Finding newFinding)
        {
            switch (command)
            {
                case "\\FindingAuthor":
                    newFinding.FindingAuthor = data;
                    break;
                case "\\FindingName":
                    newFinding.FindingName = data;
                    break;
                case "\\Location":
                    newFinding.Location = extractLocation(data);
                    break;
                case "\\Component":
                    newFinding.Component = data;
                    break;
                case "\\FoundWith":
                    newFinding.FoundWith = data;
                    break;
                case "\\TestMethod":
                    newFinding.TestMethod = data;
                    break;
                case "\\CVSS":
                    newFinding.CVSS = data;
                    break;
                case "\\CVSSvector":
                    newFinding.CVSSVector = data;
                    break;
                case "\\CWE":
                    newFinding.CWE = int.Parse(data);
                    break;
                case "\\Criticality":
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
                case "\\Exploitability":
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
                case "\\Category":
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
                case "\\Detectability":
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
                    newFinding.SubsectionDetails = data;
                    break;
                case "Impact":
                    newFinding.SubsectionImpact = data;
                    break;
                case "Repeatability":
                    newFinding.SubsectionRepeatability = data;
                    break;
                case "Countermeasures":
                    newFinding.SubsectionCountermeasures = data;
                    break;
                case "References":
                    newFinding.SubsectionReferences = data;
                    break;
            }
        }

        private List<string> extractLocation(string data)
        {
            List<string> result = data.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                result[i] = result[i].Trim();
            }

            return result;
        }

        private void extractSubsection(string command, StreamReader reader, Finding newFinding)
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
                    result += line;
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
        }

        /*private List<string> extractReferences(string data, StreamReader reader, Finding newFinding)
        {
            string line;
            List<string> resultList = new List<string>();
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
                            resultList.Add(currentItem.Trim());
                        }
                        return resultList;
                    }
                }

                if (trimmedLine.Length >= 5 && trimmedLine.Substring(0, 5) == "\\item")
                {
                    if (!firstItem)
                    {
                        resultList.Add(currentItem.Trim());
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
