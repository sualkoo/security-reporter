using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

[TestFixture]
public class DbProjectInformationExtractorTests
{
    [Test]
    public void ExtractProjectInformationTest()
    {
        // Arrange
        var projectInformation = new ProjectInformation
        {
            ApplicationManager = new ProjectInformationParticipant
            {
                Name = "Anakin Skywalker",
                Department = "SHS DI DA CEC ITH EH-PLM",
                Contact = "anakin.skywalker@siemens-healthineers.com"
            },
            BusinessOwner = new ProjectInformationParticipant
            {
                Name = "Padme Amidala",
                Department = "SHS DI DA CEC EPE",
                Contact = "padme.amidala@siemens-healthineers.com"
            },
            BusinessRepresentative = new ProjectInformationParticipant
            {
                Name = "Luke Skywalker",
                Department = "SHS DI DA CEC ITH EH-PLM",
                Contact = "luke.skywalker@siemens-healthineers.com"
            },
            TechnicalContacts = new List<ProjectInformationParticipant>()
        };
        var technicalContact = new ProjectInformationParticipant
        {
            Name = "Obi Wan Kenobi",
            Department = "SHS TE DC SVK D&A DIG PTM",
            Contact = "obi-wan.kenobi@siemens-healthineers.com"
        };
        var technicalContact2 = new ProjectInformationParticipant
        {
            Name = "Baby Yoda",
            Department = "SHS DI D&A CEC ITH EH-R&D",
            Contact = "baby.yoda@siemens-healthineers.com"
        };
        projectInformation.TechnicalContacts.Add(technicalContact);
        projectInformation.TechnicalContacts.Add(technicalContact2);

        var pentestLead = new ProjectInformationParticipant
        {
            Name = "Lukas Nad",
            Department = "SHS TE DC CYS CSA PPA",
            Contact = "lukas.nad@siemens-healthineers.com"
        };
        projectInformation.PentestLead = pentestLead;
        var pentestCoordinator = new ProjectInformationParticipant
        {
            Name = "Alzbeta Vojtusova",
            Department = "SHS TE DC CYS CSA& P&PA",
            Contact = "alzbeta.vojtusova@siemens-healthineers.com"
        };
        projectInformation.PentestCoordinator = pentestCoordinator;
        projectInformation.PentestTeam = new List<ProjectInformationParticipant>();
        var pentestTeamParticipant = new ProjectInformationParticipant
        {
            Name = "Lukas Nad",
            Department = "SHS TE DC CYS CSA P&PA",
            Contact = "lukas.nad@siemens-healthineers.com"
        };
        if (pentestTeamParticipant == null) throw new ArgumentNullException(nameof(pentestTeamParticipant));
        var pentestTeamParticipant2 = new ProjectInformationParticipant
        {
            Name = "Michal Olencin",
            Department = "SHS TE DC CYS CSA P&PA",
            Contact = "michal.olencin@siemens-healthineers.com"
        };
        projectInformation.PentestTeam.Add(pentestTeamParticipant);
        projectInformation.PentestTeam.Add(pentestTeamParticipant2);

        projectInformation.TargetInfoVersion = "12.1.1.2";
        projectInformation.TargetInfoEnvironment = "Testing Environment";
        projectInformation.TargetInfoInternetFacing = true;
        projectInformation.TargetInfoSNXConnectivity = false;
        projectInformation.TargetInfoHostingLocation = "Special Network";
        projectInformation.TargetInfoHostingProvider = "N/A";
        projectInformation.TargetInfoLifeCyclePhase = "Pre-Production";
        projectInformation.TargetInfoCriticality = "N/A";
        projectInformation.TargetInfoAssetID = "N/A";
        projectInformation.TargetInfoSHARPUUID = "N/A";
        projectInformation.TargetInfoDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

        projectInformation.TimeFrameTotal = 10;
        projectInformation.TimeFrameStart = new DateTime(2023, 5, 29);
        projectInformation.TimeFrameEnd = new DateTime(2023, 6, 9);
        projectInformation.TimeFrameReportDue = new DateTime(2023, 6, 12);
        projectInformation.TimeFrameComment = "-";

        projectInformation.FindingsCountCritical = 0;
        projectInformation.FindingsCountHigh = 1;
        projectInformation.FindingsCountMedium = 2;
        projectInformation.FindingsCountLow = 2;
        projectInformation.FindingsCountInfo = 2;
        projectInformation.FindingsCountTotal = 7;


        var expectedStr = @"%----------------------------------------------------------------------------------------
%	PROJECT INFORMATION
%----------------------------------------------------------------------------------------
\newcommand{\ApplicationManager}{Anakin Skywalker}
\newcommand{\ApplicationManagerDepartment}{SHS DI DA CEC ITH EH-PLM}
\newcommand{\ApplicationManagerContact}{\href{mailto://anakin.skywalker@siemens-healthineers.com}{anakin.skywalker\footnotemark[1]}}

\newcommand{\BusinessOwnerName}{Padme Amidala}
\newcommand{\BusinessOwnerDepartment}{SHS DI DA CEC EPE}
\newcommand{\BusinessOwnerContact}{\href{mailto://padme.amidala@siemens-healthineers.com}{padme.amidala\footnotemark[1]}}

\newcommand{\BusinessRepresentativeName}{Luke Skywalker}
\newcommand{\BusinessRepresentativeDepartment}{SHS DI DA CEC ITH EH-PLM}
\newcommand{\BusinessRepresentativeContact}{\href{mailto://luke.skywalker@siemens-healthineers.com}{luke.skywalker\footnotemark[1]}}

\newcommand{\TechnicalContactsNumber}{2}
\newcommand{\TechnicalContacts}{
	Obi Wan Kenobi & SHS TE DC SVK D\&A DIG PTM & \href{mailto://obi-wan.kenobi@siemens-healthineers.com}{obi-wan.kenobi\footnotemark[1]} \\ &
	Baby Yoda & SHS DI D\&A CEC ITH EH-R\&D & \href{mailto://baby.yoda@siemens-healthineers.com}{baby.yoda\footnotemark[1]} \\
}

% Not needed for Scope document
% Required for Report document
\newcommand{\PentestLeadName}{Lukas Nad}
\newcommand{\PentestLeadDepartment}{SHS TE DC CYS CSA PPA}
\newcommand{\PentestLeadContact}{\href{mailto://lukas.nad@siemens-healthineers.com}{lukas.nad\footnotemark[1]}}

\newcommand{\PentestCoordinatorName}{Alzbeta Vojtusova}
\newcommand{\PentestCoordinatorDepartment}{SHS TE DC CYS CSA\& P\&PA}
\newcommand{\PentestCoordinatorContact}{\href{mailto://alzbeta.vojtusova@siemens-healthineers.com}{alzbeta.vojtusova\footnotemark[1]}}



\newcommand{\PentestParticipantsNumber}{2} % Number of participants in ""Penetration Testing Team""
\newcommand{\PentestTeamMember}{
	Lukas Nad & SHS TE DC CYS CSA P\&PA & \href{mailto://lukas.nad@siemens-healthineers.com}{lukas.nad\footnotemark[1]} \\ &
	Michal Olencin & SHS TE DC CYS CSA P\&PA & \href{mailto://michal.olencin@siemens-healthineers.com}{michal.olencin\footnotemark[1]} \\
}

%----------------------------------------------------------------------------------------
%	TARGET INFORMATION
%----------------------------------------------------------------------------------------
\newcommand{\TargetInfoName}{\ReportProjectName} %% Asset Name
\newcommand{\TargetInfoVersion}{12.1.1.2} %% Asset Version 	
\newcommand{\TargetInfoType}{\AssetType} %% Asset Type
\newcommand{\TargetInfoEnvironment}{Testing Environment}
\newcommand{\TargetInfoInternetFacing}{Yes} %% Asset Internet Facing
\newcommand{\TargetInfoSNXConnectivity}{No} %% SNX Connectivity
\newcommand{\TargetInfoHostingLocation}{Special Network} %% Hosting Location
\newcommand{\TargetInfoHostingProvider}{N/A} %% Hosting Provider
\newcommand{\TargetInfoLifecyclePhase}{Pre-Production}
\newcommand{\TargetInfoCriticality}{N/A}
\newcommand{\TargetInfoAssetID}{N/A}
\newcommand{\TargetInfoSHARPUUID}{N/A} %% SHARP UUID
\newcommand{\TargetInfoDescription}{Lorem ipsum dolor sit amet, consectetur adipiscing elit.}

%----------------------------------------------------------------------------------------
%	AGREED TIMEFRAME
%----------------------------------------------------------------------------------------
\newcommand{\TimeframeTotal}{10 working days} 
\newcommand{\TimeframeStart}{2023-05-29} 
\newcommand{\TimeframeEnd}{2023-06-09} 
\newcommand{\TimeframeReportDue}{2023-06-12} 
\newcommand{\TimeframeComment}{-}

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

\newcommand{\FindingsCountCritical}{0}
\newcommand{\FindingsCountHigh}{1}
\newcommand{\FindingsCountMedium}{2}
\newcommand{\FindingsCountLow}{2}
\newcommand{\FindingsCountInfo}{2}
\newcommand{\FindingsCountTotal}{7}";

        // Act
        var result = DbProjectInformationExtractor.ExtractProjectInformation(projectInformation);
        var resultDecoded = Encoding.UTF8.GetString(result);

        // Assert
        Assert.IsNotNull(result);
        StringAssert.Contains(StringNormalizer.Normalize(expectedStr), StringNormalizer.Normalize(resultDecoded));
    }
}