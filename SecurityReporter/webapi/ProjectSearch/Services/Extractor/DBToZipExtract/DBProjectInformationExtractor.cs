using System.Text;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DBProjectInformationExtractor
    {
        public byte[] extractProjectInformation(ProjectInformation projectInformation)
        {
			var technicalContactsParsed = new List<string>();
			if (projectInformation.TechnicalContacts != null) {
				foreach (ProjectInformationParticipant tc in projectInformation.TechnicalContacts)
				{
					string tcString = "\t" + tc.Name + " & " + tc.Department + " & \\href{" + tc.Contact + "}{" + tc.Contact!.Split("@")[0] + "\\footnotemark[1]} \\\\";
					technicalContactsParsed.Add(tcString);
				}
			}

            var pentestTeamParsed = new List<string>();
            if (projectInformation.PentestTeam != null)
            {
                foreach (ProjectInformationParticipant tc in projectInformation.PentestTeam)
                {
                    string tcString = "\t" + tc.Name + " & " + tc.Department + " & \\href{" + tc.Contact + "}{" + tc.Contact!.Split("@")[0] + "\\footnotemark[1]} \\\\";
                    pentestTeamParsed.Add(tcString);
                }
            }

            string content =				
@"%----------------------------------------------------------------------------------------
%	PROJECT INFORMATION
%----------------------------------------------------------------------------------------
\newcommand{\ApplicationManager}{" + projectInformation.ApplicationManager!.Name + @"}
\newcommand{\ApplicationManagerDepartment}{" + projectInformation.ApplicationManager.Department + @"}
\newcommand{\ApplicationManagerContact}{\href{mailto://" + projectInformation.ApplicationManager.Contact + @"}{" + projectInformation.ApplicationManager.Contact!.Split("@")[0] + @"\footnotemark[1]}}

\newcommand{\BusinessOwnerName}{" + projectInformation.BusinessOwner!.Name + @"}
\newcommand{\BusinessOwnerDepartment}{" + projectInformation.BusinessOwner!.Department + @"}
\newcommand{\BusinessOwnerContact}{\href{mailto://" + projectInformation.BusinessOwner!.Contact + @"}{" + projectInformation.BusinessOwner.Contact!.Split("@")[0] + @"\footnotemark[1]}}

\newcommand{\BusinessRepresentativeName}{" + projectInformation.BusinessRepresentative!.Name + @"}
\newcommand{\BusinessRepresentativeDepartment}{" + projectInformation.BusinessRepresentative!.Department + @"}
\newcommand{\BusinessRepresentativeContact}{\href{mailto://" + projectInformation.BusinessRepresentative!.Contact + @"}{" + projectInformation.BusinessRepresentative.Contact!.Split("@")[0] + @"\footnotemark[1]}}

\newcommand{\TechnicalContactsNumber}{" + technicalContactsParsed.Count() + @"}
\newcommand{\TechnicalContacts}{
" + string.Join("\u0020&\n", technicalContactsParsed) + @"
}

% Not needed for Scope document
% Required for Report document
\newcommand{\PentestLeadName}{" + projectInformation.PentestLead!.Name + @"}
\newcommand{\PentestLeadDepartment}{" + projectInformation.PentestLead!.Department + @"}
\newcommand{\PentestLeadContact}{\href{mailto://" + projectInformation.PentestLead!.Contact + @"}{" + projectInformation.PentestLead!.Contact!.Split("@")[0] + @"\footnotemark[1]}}

\newcommand{\PentestCoordinatorName}{" + projectInformation.PentestCoordinator!.Name + @"}
\newcommand{\PentestCoordinatorDepartment}{" + projectInformation.PentestCoordinator!.Department + @"}
\newcommand{\PentestCoordinatorContact}{\href{mailto://" + projectInformation.PentestCoordinator!.Contact + @"}{" + projectInformation.PentestCoordinator!.Contact!.Split("@")[0] + @"\footnotemark[1]}}



\newcommand{\PentestParticipantsNumber}{" + pentestTeamParsed.Count() + @"} % Number of participants in ""Penetration Testing Team""
\newcommand{\PentestTeamMember}{
" + string.Join("\u0020&\n", pentestTeamParsed) + @"
}

%----------------------------------------------------------------------------------------
%	TARGET INFORMATION
%----------------------------------------------------------------------------------------
\newcommand{\TargetInfoName}{\ReportProjectName} %% Asset Name
\newcommand{\TargetInfoVersion}{" + projectInformation.TargetInfoVersion + @"} %% Asset Version 	
\newcommand{\TargetInfoType}{\AssetType} %% Asset Type
\newcommand{\TargetInfoEnvironment}{" + projectInformation.TargetInfoEnvironment + @"}
\newcommand{\TargetInfoInternetFacing}{" + (projectInformation.TargetInfoInternetFacing ? "Yes" : "No") + @"} %% Asset Internet Facing
\newcommand{\TargetInfoSNXConnectivity}{" + (projectInformation.TargetInfoSNXConnectivity ? "Yes" : "No") + @"} %% SNX Connectivity
\newcommand{\TargetInfoHostingLocation}{" + projectInformation.TargetInfoHostingConnection + @"} %% Hosting Location
\newcommand{\TargetInfoHostingProvider}{" + projectInformation.TargetInfoHostingProvider + @"} %% Hosting Provider
\newcommand{\TargetInfoLifecyclePhase}{" + projectInformation.TargetInfoLifeCyclePhase + @"}
\newcommand{\TargetInfoCriticality}{" + projectInformation.TargetInfoCriticality + @"}
\newcommand{\TargetInfoAssetID}{" + projectInformation.TargetInfoAssetID + @"}
\newcommand{\TargetInfoSHARPUUID}{" + projectInformation.TargetInfoSHARPUUID + @"} %% SHARP UUID
\newcommand{\TargetInfoDescription}{" + projectInformation.TargetInfoDescription + @"}

%----------------------------------------------------------------------------------------
%	AGREED TIMEFRAME
%----------------------------------------------------------------------------------------
\newcommand{\TimeframeTotal}{" + projectInformation.TimeFrameTotal +  @" working days} 
\newcommand{\TimeframeStart}{" + projectInformation.TimeFrameStart.ToString("yyyy-MM-dd") +  @"} 
\newcommand{\TimeframeEnd}{" + projectInformation.TimeFrameEnd.ToString("yyyy-MM-dd") +  @"} 
\newcommand{\TimeframeReportDue}{" + projectInformation.TimeFrameReportDue.ToString("yyyy-MM-dd") +  @"} 
\newcommand{\TimeframeComment}{" + projectInformation.TimeFrameComment +  @"}

%----------------------------------------------------------------------------------------
%	FINDINGS COUNT AND OVERALL THREAT EXPOSURE
%----------------------------------------------------------------------------------------
% Not needed for Scope document
% Required for Report document

\newcommand{\OverallThreatExposureImage}{\includegraphics{Images/CriticalThreat.png}}
% \includegraphics{Images/CriticalThreat.png}, 
% \includegraphics{Images/HighThreat.png}, 
% \includegraphics{Images/MediumThreat.png}, 
% \includegraphics{Images/LowThreat.png}

\newcommand{\FindingsCountCritical}{" + projectInformation.FindingsCountCritical + @"}
\newcommand{\FindingsCountHigh}{" + projectInformation.FindingsCountHigh + @"}
\newcommand{\FindingsCountMedium}{" + projectInformation.FindingsCountMedium + @"}
\newcommand{\FindingsCountLow}{" + projectInformation.FindingsCountLow + @"}
\newcommand{\FindingsCountInfo}{" + projectInformation.FindingsCountInfo + @"}
\newcommand{\FindingsCountTotal}{" + projectInformation.FindingsCountTotal + @"}";
			Console.Write(content);
            return Encoding.UTF8.GetBytes(content);
        }
    }
}
