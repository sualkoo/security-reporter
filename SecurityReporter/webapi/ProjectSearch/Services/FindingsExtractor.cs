using System.IO.Compression;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services
{
    public class FindingsExtractor
    {
        ZipArchiveEntry currentEntry;
        ZipArchiveEntry findingsDirectory;
        List<Finding> findingsList = new List<Finding>();
        public Finding extractFinding(ZipArchiveEntry currentEntry)
        {
            Finding newFinding = new Finding();
            string line;
            char[] delimiters = { '{', '}' };
            using (StreamReader reader = new StreamReader(currentEntry.Open()))
            {
                while ((line = reader.ReadLine()) != null) {
                    if (!string.IsNullOrEmpty(line) && line.Trim().Length > 0 && line.Trim()[0] == '\\')
                    {
                        string[] splitString = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        if (splitString.Length == 3)
                        {
                            assignNewData(splitString[1], splitString[2], newFinding);
                        } else if (splitString.Length == 2)
                        {
                            assignNewData(splitString[1], extractSubsection(splitString[1]), newFinding);
                        }
                    }
                }

                return newFinding;
            }
        }

        private void assignNewData(string command, string data, Finding newFinding)
        {
            switch(command)
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
                case "\\FoundWitch":
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
                    newFinding.CWE = data;
                    break;
                case "\\Criticality":
                    newFinding.Criticality = 0;
                    break;
                case "\\Exploitability":
                    newFinding.Exploitability = 0;
                    break;
                case "\\Category":
                    newFinding.Category = 0;
                    break;
                case "\\Detectability":
                    newFinding.Detectability = 0;
                    break;
                case "\\Details":
                    newFinding.SubsectionDetails = data;
                    break;
                case "\\Imapct":
                    newFinding.SubsectionImpact = data;
                    break;
                case "\\Repeatability":
                    newFinding.SubsectionRepeatability = data;
                    break;
                case "\\Countermeasures":
                    newFinding.SubsectionCountermeasures = data;
                    break;
                case "\\References":
                    newFinding.SubsectionReferences = extractReferences(data);
                    break;
            }
        }

        private List<string> extractLocation(string data)
        {
            return data.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private List<string> extractReferences(string data)
        {
            return new List<string> {  data };

        }

        private string extractSubsection(string command)
        {
            string result = "";
            return result;

        }
    }
}
