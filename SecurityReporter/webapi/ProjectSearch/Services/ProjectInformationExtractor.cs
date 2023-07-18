using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services
{
    public class ProjectInformationExtractor
    {
        ZipArchiveEntry projectInfoEntry;
        Dictionary<string, ProjectInformationParticipant> pentestTeamDictionary;
        ProjectInformation newProjectInfo = new ProjectInformation();
        DateTime newReportDate;
        public ProjectInformationExtractor(ZipArchiveEntry projectInfoEntry, 
            Dictionary<string, ProjectInformationParticipant> pentestTeamDictionary) {
            this.projectInfoEntry = projectInfoEntry;
            this.pentestTeamDictionary = pentestTeamDictionary;
        }

        public ProjectInformation ExtractProjectInformation()
        {
            string line;
            if(projectInfoEntry == null)
            {
                throw new ArgumentNullException();
            } else
            {
                using (StreamReader reader = new StreamReader(projectInfoEntry.Open()))
                {
                    char[] delimiters = { '{', '}'};
                    while((line = reader.ReadLine()) != null)
                    {
                        if(!string.IsNullOrEmpty(line) && (line[0] == '\\' /*|| line[0] == '\t'*/))
                        {
                            string[] inBracketContents = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            List<string> result;
                            if(inBracketContents.Length == 3 || inBracketContents.Length == 4)
                            {
                                AssignNewData(inBracketContents[1], inBracketContents[2]);
                                
                            } else if(inBracketContents.Length > 4)
                            {
                                AssignNewData(inBracketContents[1], inBracketContents[3]);
                            }
                        }
                    }
                }
            }

            return newProjectInfo;
        }

        private string extractDepartment(string data)
        {
            string result = "";
            string delimiter = "\\&";
            string[] editedString = data.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach(string str in editedString)
            {
                result += str;
            }

            return result;
        }

        private string extractContact(string data)
        {
            return data.Substring(9);
        }

        private void AssignNewData(string command, string data)
        {
            switch(command)
            {
                case "\\ApplicationManager":
                    newProjectInfo.ApplicationManager = new ProjectInformationParticipant();
                    newProjectInfo.ApplicationManager.Name = data;
                    newProjectInfo.ApplicationManager.Role = Enums.ProjectParticipantRole.ApplicationManager;
                    break;
                case "\\ApplicationManagerDepartment":
                    newProjectInfo.ApplicationManager.Department = extractDepartment(data);
                    break;
                case "\\ApplicationManagerContact":
                    newProjectInfo.ApplicationManager.Contact = extractContact(data);
                    break;
                case "\\BusinessOwnerName":
                    newProjectInfo.BusinessOwner = new ProjectInformationParticipant();
                    newProjectInfo.BusinessOwner.Name = data;
                    newProjectInfo.BusinessOwner.Role = Enums.ProjectParticipantRole.BusinessOwner;
                    break;
                case "\\BusinessOwnerDepartment":
                    newProjectInfo.BusinessOwner.Department = extractDepartment(data);
                    break;
                case "\\BusinessOwnerContact":
                    newProjectInfo.BusinessOwner.Contact = extractContact(data);
                    break;
                case "\\BusinessRepresentativeName":
                    newProjectInfo.BusinessRepresentative = new ProjectInformationParticipant();
                    newProjectInfo.BusinessRepresentative.Name = data;
                    newProjectInfo.BusinessRepresentative.Role = Enums.ProjectParticipantRole.BusinessRepresentative;
                    break;
                case "\\BusinessRepresentativeDepartment":
                    newProjectInfo.BusinessRepresentative.Department = extractDepartment(data);
                    break;
                case "\\BusinessRepresentativeContact":
                    newProjectInfo.BusinessRepresentative.Contact = extractContact(data);
                    break;
                case "\\PentestLeadName":
                    newProjectInfo.PentestLead = new ProjectInformationParticipant();
                    newProjectInfo.PentestLead.Name = data;
                    newProjectInfo.PentestLead.Role = Enums.ProjectParticipantRole.PentestLead;
                    break;
                case "\\PentestLeadDepartment":
                    newProjectInfo.PentestLead.Department = extractDepartment(data);
                    break;
                case "\\PentestLeadContact":
                    newProjectInfo.PentestLead.Contact = extractContact(data);
                    break;
                case "\\PentestCoordinatorName":
                    newProjectInfo.PentestCoordinator = new ProjectInformationParticipant();
                    newProjectInfo.PentestCoordinator.Name = data;
                    newProjectInfo.PentestCoordinator.Role = Enums.ProjectParticipantRole.PentestCoordinator;
                    break;
                case "\\PentestCoordinatorDepartment":
                    newProjectInfo.PentestCoordinator.Department = extractDepartment(data);
                    break;
                case "\\PentestCoordinatorContact":
                    newProjectInfo.PentestCoordinator.Contact = extractContact(data);
                    break;
                case "\\TargetInfoVersion":
                    newProjectInfo.TargetInfoVersion = data;
                    break;
                case "\\TargetInfoEnvironment":
                    newProjectInfo.TargetInfoEnviroment = data;
                    break;
                case "\\TargetInfoInternetFacing":
                    if(data == "Yes")
                    {
                        newProjectInfo.TargetInfoInternetFacing = true;
                    } else if(data == "No")
                    {
                        newProjectInfo.TargetInfoInternetFacing = false;
                    }
                    break;
                case "\\TargetInfoSNXConnectivity":
                    if (data == "Yes")
                    {
                        newProjectInfo.TargetInfoSNXConectivity = true;
                    }
                    else if (data == "No")
                    {
                        newProjectInfo.TargetInfoSNXConectivity = false;
                    }
                    break;
                case "\\TargetInfoHostingLocation":
                    newProjectInfo.TargetInfoHostingConnection = data;
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

                    foreach(Match match in matches)
                    {
                        resultString += match.Value;
                    }

                    newProjectInfo.TimeFrameTotal = int.Parse(resultString);
                    break;

                case "\\TimeframeStart":
                    newReportDate = DateTime.ParseExact(data, "yyyy-MM-d", CultureInfo.InvariantCulture);
                    newProjectInfo.TimeFrameStart = new DateOnly(newReportDate.Year, newReportDate.Month, newReportDate.Day);
                    break;

                case "\\TimeframeEnd":
                    newReportDate = DateTime.ParseExact(data, "yyyy-MM-d", CultureInfo.InvariantCulture);
                    newProjectInfo.TimeFrameEnd = new DateOnly(newReportDate.Year, newReportDate.Month, newReportDate.Day);
                    break;
                case "\\TimeframeReportDue":
                    newReportDate = DateTime.ParseExact(data, "yyyy-MM-d", CultureInfo.InvariantCulture);
                    newProjectInfo.TimeFrameReportDue = new DateOnly(newReportDate.Year, newReportDate.Month, newReportDate.Day);
                    break;
                case "\\TimeframeComment":
                    newProjectInfo.TimeFrameComment = data;
                    break;
                case "\\FindingsCountCritical":
                    newProjectInfo.FindingsCountCritical = int.Parse(data);
                    break;
                case "\\FindingsCountHigh":
                    newProjectInfo.FindingsCountCriticalHigh = int.Parse(data);
                    break;
                case "\\FindingsCountMedium":
                    newProjectInfo.FindingsCountCriticalMedium = int.Parse(data);
                    break;
                case "\\FindingsCountLow":
                    newProjectInfo.FindingsCountCriticalLow = int.Parse(data);
                    break;
                case "\\FindingsCountInfo":
                    newProjectInfo.FindingsCountCriticalInfo = int.Parse(data);
                    break;
                case "\\FindingsCountTotal":
                    newProjectInfo.FindingsCountCriticalTotal = int.Parse(data);
                    break;
                case "\\FindingsCountTBD":
                    newProjectInfo.FindingsCountCriticalTBD = int.Parse(data);
                    break;

            }
        }


    }

    
}
