using System.IO.Compression;
using webapi.Enums;
using webapi.ProjectSearch.Enums;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

public class FindingsExtractor
{
    private ZipArchiveEntry findingsDirectory;
    private List<Finding> findingsList = new();

    public Finding ExtractFinding(ZipArchiveEntry currentEntry)
    {
        var newFinding = new Finding();
        string line;
        char[] delimiters = { '{', '}' };
        using (var reader = new StreamReader(currentEntry.Open()))
        {
            while ((line = reader.ReadLine()) != null)
                if (!string.IsNullOrEmpty(line) && line.Trim().Length > 0 && line.Trim()[0] == '\\')
                {
                    var splitString = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (splitString.Length == 3)
                        AssignNewData(splitString[1], splitString[2], newFinding);
                    else if (splitString.Length == 2)
                        if (splitString[0] == "\\subsection*")
                            ExtractSubsection(splitString[1], reader, newFinding);
                }

            return newFinding;
        }
    }

    private void AssignNewData(string command, string data, Finding newFinding)
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
                        newFinding.Criticality = Criticality.High;
                        break;
                    case "Medium":
                        newFinding.Criticality = Criticality.Medium;
                        break;
                    case "Low":
                        newFinding.Criticality = Criticality.Low;
                        break;
                    case "Info":
                        newFinding.Criticality = Criticality.Info;
                        break;
                    case "To Be Rated":
                        newFinding.Criticality = Criticality.TBR;
                        break;
                }

                break;
            case "\\Exploitability":
                switch (data)
                {
                    case "Easy":
                        newFinding.Exploitability = Exploitability.Easy;
                        break;
                    case "Average":
                        newFinding.Exploitability = Exploitability.Average;
                        break;
                    case "Hard":
                        newFinding.Exploitability = Exploitability.Hard;
                        break;
                    case "To Be Rated":
                        newFinding.Exploitability = Exploitability.TBR;
                        break;
                }

                break;
            case "\\Category":
                switch (data)
                {
                    case "Access Control":
                        newFinding.Category = Category.AccessControl;
                        break;
                    case "Application Design":
                        newFinding.Category = Category.ApplicationDesign;
                        break;
                    case "Information Disclosure":
                        newFinding.Category = Category.InformationDisclosure;
                        break;
                    case "Outdated Software":
                        newFinding.Category = Category.OutdatedSoftware;
                        break;
                    case "Security Configuration":
                        newFinding.Category = Category.SecurityConfiguration;
                        break;
                }

                break;
            case "\\Detectability":
                switch (data)
                {
                    case "Easy":
                        newFinding.Detectability = Detectability.Easy;
                        break;
                    case "Average":
                        newFinding.Detectability = Detectability.Average;
                        break;
                    case "Difficult":
                        newFinding.Detectability = Detectability.Difficult;
                        break;
                    case "To Be Rated":
                        newFinding.Detectability = Detectability.TBR;
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

    private static List<string> extractLocation(string data)
    {
        var result = data.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        for (var i = 0; i < result.Count; i++) result[i] = result[i].Trim();

        return result;
    }

    private void ExtractSubsection(string command, StreamReader reader, Finding newFinding)
    {
        string line;
        var reading = true;
        var result = "";
        while ((line = reader.ReadLine()) != null && reading)
        {
            var trimmedLine = line.Trim();
            //reading = trimmedLine.Length >= 6 && trimmedLine.Substring(0, 6) == "\\begin" ? false : true;
            reading = trimmedLine.Length >= 11 && trimmedLine.Substring(0, 11) == "\\subsection" ? false : true;
            //reading = trimmedLine.Length > 0 && trimmedLine[0] == '%' ? false : true;

            if (reading)
            {
                result += line + '\n';
            }
            else
            {
                AssignNewData(command, result, newFinding);
                if (trimmedLine.Length >= 12 && trimmedLine.Substring(0, 11) == "\\subsection")
                {
                    char[] delimiters = { '{', '}' };
                    var splitString = trimmedLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (splitString.Length == 2) ExtractSubsection(splitString[1], reader, newFinding);
                }
            }
        }

        if (line == null) AssignNewData(command, result, newFinding);
    }

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