using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract
{
    public class ProjectInformationExtractor
    {
        ZipArchiveEntry projectInfoEntry;
        Dictionary<string, ProjectInformationParticipant> pentestTeamDictionary;
        ProjectInformation newProjectInfo = new ProjectInformation();
        bool technicalContacts = false;
        bool pentestTeamMember = false;
        public ProjectInformationExtractor(ZipArchiveEntry projectInfoEntry,
            Dictionary<string, ProjectInformationParticipant> pentestTeamDictionary)
        {
            this.projectInfoEntry = projectInfoEntry;
            this.pentestTeamDictionary = pentestTeamDictionary;
        }

        public ProjectInformation ExtractProjectInformation()
        {
            string line;
            newProjectInfo.PentestTeam = new List<ProjectInformationParticipant>();
            newProjectInfo.TechnicalContacts = new List<ProjectInformationParticipant>();

            if (projectInfoEntry == null)
            {
                throw new ArgumentNullException();
            }

            using (StreamReader reader = new StreamReader(projectInfoEntry.Open()))
            {
                string fileContent = reader.ReadToEnd();
                string regexPattern = @"\\(newcommand|renewcommand)\s*\{\\([a-zA-Z]*)\}\s*\{((?>[^{}]+|\{(?<DEPTH>)|\}(?<-DEPTH>))*(?(DEPTH)(?!)))\}";

                MatchCollection matches = Regex.Matches(fileContent, regexPattern);
                foreach (Match match in matches)
                {
                    if (match.Groups[2].Value == "PentestTeamMember" || match.Groups[2].Value == "TechnicalContacts")
                    {
                        string memberPattern = @"\s*[\w\W]*?(?<=\s|[\\\s&])$";

                        MatchCollection memberMatches = Regex.Matches(match.Groups[3].Value, memberPattern, RegexOptions.Multiline);
                        foreach(Match member in memberMatches)
                        {
                            if(!String.IsNullOrWhiteSpace(member.Value))
                            {
                                AssignNewData(match.Groups[2].Value.Trim(), member.Value);
                            }
                        }
                    } else
                    {
                        AssignNewData(match.Groups[2].Value, match.Groups[3].Value);
                    }
                }

                /*char[] delimiters = { '{', '}' };
                    string[] listsDelimiter = { "\\\\" };
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line) && (line[0] == '\\' || line[0] == '\t'))
                        {
                            string[] pentestTeamMemberContents = line.Split(listsDelimiter, StringSplitOptions.None);
                            if (pentestTeamMemberContents.Length == 2)
                            {
                                if (technicalContacts)
                                {
                                    AssignNewData("\\TechnicalContacts", pentestTeamMemberContents[0].Trim());
                                }
                                else if (pentestTeamMember)
                                {
                                    AssignNewData("\\PentestTeamMember", pentestTeamMemberContents[0].Trim());
                                }
                            }
                            else
                            {
                                string[] inBracketContents = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                                if (inBracketContents.Length == 2)
                                {
                                    pentestTeamMember = inBracketContents[1] == "\\PentestTeamMember" ? true : false;
                                    technicalContacts = inBracketContents[1] == "\\TechnicalContacts" ? true : false;
                                }
                                else if (inBracketContents.Length == 3 || inBracketContents.Length == 4)
                                {
                                    AssignNewData(inBracketContents[1], inBracketContents[2]);

                                }
                                else if (inBracketContents.Length > 4)
                                {
                                    AssignNewData(inBracketContents[1], inBracketContents[3]);
                                }
                            }

                        }
                    }*/
            }

            return newProjectInfo;
        }

        private string extractDepartment(string data)
        {
            if (data != null)
            {
                string result = "";
                string delimiter = "\\&";
                string[] editedString = data.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in editedString)
                {
                    result += str;
                }

                return result;
            }

            return null;

        }

        private string extractContact(string data)
        {
            if (data != null)
            {
                return data.Substring(9);
            }

            return null;
        }

        private void AssignNewData(string command, string data)
        {
            if (data != null && command != null)
            {
                switch ("\\" + command)
                {
                    case "\\ApplicationManager":
                        newProjectInfo.ApplicationManager = new ProjectInformationParticipant();
                        newProjectInfo.ApplicationManager.Name = data;
                        pentestTeamDictionary.Add(command, newProjectInfo.ApplicationManager);
                        break;
                    case "\\ApplicationManagerDepartment":
                        if (newProjectInfo.ApplicationManager != null)
                        {
                            newProjectInfo.ApplicationManager.Department = extractDepartment(data);
                        }
                        break;
                    case "\\ApplicationManagerContact":
                        if (newProjectInfo.ApplicationManager != null)
                        {
                            newProjectInfo.ApplicationManager.Contact = extractContact(data);
                        }
                        break;
                    case "\\BusinessOwnerName":
                        newProjectInfo.BusinessOwner = new ProjectInformationParticipant();
                        newProjectInfo.BusinessOwner.Name = data;
                        pentestTeamDictionary.Add(command, newProjectInfo.BusinessOwner);
                        break;
                    case "\\BusinessOwnerDepartment":
                        if (newProjectInfo.BusinessOwner != null)
                        {
                            newProjectInfo.BusinessOwner.Department = extractDepartment(data);
                        }
                        break;
                    case "\\BusinessOwnerContact":
                        if (newProjectInfo.BusinessOwner != null)
                        {
                            newProjectInfo.BusinessOwner.Contact = extractContact(data);
                        }
                        break;
                    case "\\BusinessRepresentativeName":
                        newProjectInfo.BusinessRepresentative = new ProjectInformationParticipant();
                        newProjectInfo.BusinessRepresentative.Name = data;
                        pentestTeamDictionary.Add(command, newProjectInfo.BusinessRepresentative);
                        break;
                    case "\\BusinessRepresentativeDepartment":
                        if (newProjectInfo.BusinessRepresentative != null)
                        {
                            newProjectInfo.BusinessRepresentative.Department = extractDepartment(data);
                        }
                        break;
                    case "\\BusinessRepresentativeContact":
                        if (newProjectInfo.BusinessRepresentative != null)
                        {
                            newProjectInfo.BusinessRepresentative.Contact = extractContact(data);
                        }
                        break;
                    case "\\TechnicalContacts":
                        if (data[0] == '\\')
                        {
                            string regPattern = @"\\(?=\w)\\?\w+";
                            MatchCollection collection = Regex.Matches(data, regPattern);
                            foreach (Match match in collection)
                            {
                                newProjectInfo.PentestTeam.Add(pentestTeamDictionary[match.Value]);
                            }
                        }
                        else
                        {
                            char[] delimiters = { '{', '}' };
                            string[] splitByAmpersand = Regex.Split(data, @"(?<!\\)&")
                                .Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                            ProjectInformationParticipant newPerson = new ProjectInformationParticipant();
                            newPerson.Name = splitByAmpersand[0].Trim();
                            newPerson.Department = splitByAmpersand[1].Trim().Replace("\\", "");
                            newPerson.Contact = extractContact(
                                splitByAmpersand[2].Trim()
                                .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)[1]);
                            newProjectInfo.TechnicalContacts.Add(newPerson);
                        }

                        break;
                    case "\\PentestLeadName":
                        newProjectInfo.PentestLead = new ProjectInformationParticipant();
                        newProjectInfo.PentestLead.Name = data;
                        pentestTeamDictionary.Add(command, newProjectInfo.PentestLead);
                        break;
                    case "\\PentestLeadDepartment":
                        if (newProjectInfo.PentestLead != null)
                        {
                            newProjectInfo.PentestLead.Department = extractDepartment(data);
                        }
                        break;
                    case "\\PentestLeadContact":
                        if (newProjectInfo.PentestLead != null)
                        {
                            newProjectInfo.PentestLead.Contact = extractContact(data);
                        }
                        break;
                    case "\\PentestCoordinatorName":
                        newProjectInfo.PentestCoordinator = new ProjectInformationParticipant();
                        newProjectInfo.PentestCoordinator.Name = data;
                        pentestTeamDictionary.Add(command, newProjectInfo.PentestCoordinator);
                        break;
                    case "\\PentestCoordinatorDepartment":
                        if (newProjectInfo.PentestCoordinator != null)
                        {
                            newProjectInfo.PentestCoordinator.Department = extractDepartment(data);
                        }
                        break;
                    case "\\PentestCoordinatorContact":
                        if (newProjectInfo.PentestCoordinator != null)
                        {
                            newProjectInfo.PentestCoordinator.Contact = extractContact(data);
                        }
                        break;
                    case "\\PentestTeamMember":
                        if (data.Trim()[0] == '\\')
                        {
                            string regPattern = @"\\(?=\w)\\?\w+";
                            MatchCollection collection = Regex.Matches(data, regPattern);
                            foreach(Match match in collection)
                            {
                                newProjectInfo.PentestTeam.Add(pentestTeamDictionary[match.Value]);
                            }
                        }
                        else
                        {
                            char[] delimiters = { '{', '}' };
                            string[] splitByAmpersand = Regex.Split(data, @"(?<!\\)&")
                                .Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                            ProjectInformationParticipant newPerson = new ProjectInformationParticipant();
                            newPerson.Name = splitByAmpersand[0].Trim();
                            newPerson.Department = splitByAmpersand[1].Trim().Replace("\\", "");
                            newPerson.Contact = extractContact(
                                splitByAmpersand[2].Trim()
                                .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)[1]);
                            newProjectInfo.PentestTeam.Add(newPerson);
                        }

                        break;
                    case "\\TargetInfoVersion":
                        newProjectInfo.TargetInfoVersion = data;
                        break;
                    case "\\TargetInfoEnvironment":
                        newProjectInfo.TargetInfoEnvironment = data;
                        break;
                    case "\\TargetInfoInternetFacing":
                        if (data == "Yes")
                        {
                            newProjectInfo.TargetInfoInternetFacing = true;
                        }
                        else if (data == "No")
                        {
                            newProjectInfo.TargetInfoInternetFacing = false;
                        }
                        break;
                    case "\\TargetInfoSNXConnectivity":
                        if (data == "Yes")
                        {
                            newProjectInfo.TargetInfoSNXConnectivity = true;
                        }
                        else if (data == "No")
                        {
                            newProjectInfo.TargetInfoSNXConnectivity = false;
                        }
                        break;
                    case "\\TargetInfoHostingLocation":
                        newProjectInfo.TargetInfoHostingLocation = data;
                        break;
                    case "\\TargetInfoHostingProvider":
                        newProjectInfo.TargetInfoHostingProvider = data;
                        break;
                    case "\\TargetInfoLifecyclePhase":
                        newProjectInfo.TargetInfoLifeCyclePhase = data;
                        break;
                    case "\\TargetInfoCriticality":
                        newProjectInfo.TargetInfoCriticality = data;
                        break;
                    case "\\TargetInfoAssetID":
                        newProjectInfo.TargetInfoAssetID = data;
                        break;
                    case "\\TargetInfoSHARPUUID":
                        newProjectInfo.TargetInfoSHARPUUID = data;
                        break;
                    case "\\TargetInfoDescription":
                        newProjectInfo.TargetInfoDescription = data;
                        break;
                    case "\\TimeframeTotal":
                        string resultString = "";
                        Regex regex = new Regex(@"\d+");
                        MatchCollection matches = regex.Matches(data);

                        foreach (Match match in matches)
                        {
                            resultString += match.Value;
                        }

                        newProjectInfo.TimeFrameTotal = int.Parse(resultString);
                        break;

                    case "\\TimeframeStart":
                        newProjectInfo.TimeFrameStart = DateTime.ParseExact(data, "yyyy-MM-d", CultureInfo.InvariantCulture);
                        break;

                    case "\\TimeframeEnd":
                        newProjectInfo.TimeFrameEnd = DateTime.ParseExact(data, "yyyy-MM-d", CultureInfo.InvariantCulture);
                        break;
                    case "\\TimeframeReportDue":
                        newProjectInfo.TimeFrameReportDue = DateTime.ParseExact(data, "yyyy-MM-d", CultureInfo.InvariantCulture); ;
                        break;
                    case "\\TimeframeComment":
                        newProjectInfo.TimeFrameComment = data;
                        break;
                    case "\\FindingsCountCritical":
                        newProjectInfo.FindingsCountCritical = int.Parse(data);
                        break;
                    case "\\FindingsCountHigh":
                        newProjectInfo.FindingsCountHigh = int.Parse(data);
                        break;
                    case "\\FindingsCountMedium":
                        newProjectInfo.FindingsCountMedium = int.Parse(data);
                        break;
                    case "\\FindingsCountLow":
                        newProjectInfo.FindingsCountLow = int.Parse(data);
                        break;
                    case "\\FindingsCountInfo":
                        newProjectInfo.FindingsCountInfo = int.Parse(data);
                        break;
                    case "\\FindingsCountTotal":
                        newProjectInfo.FindingsCountTotal = int.Parse(data);
                        break;
                    case "\\FindingsCountTBD":
                        newProjectInfo.FindingsCountCriticalTBD = int.Parse(data);
                        break;
                }
            }
        }
    }
}
