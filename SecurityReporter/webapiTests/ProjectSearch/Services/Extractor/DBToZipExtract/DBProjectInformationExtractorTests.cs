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

        // Act
        var result = DbProjectInformationExtractor.ExtractProjectInformation(projectInformation);
        var resultDecoded = Encoding.UTF8.GetString(result);

        // Assert
        Assert.IsNotNull(result);
        StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tPROJECT INFORMATION\r\n%----------------------------------------------------------------------------------------\r\n\\newcommand{\\ApplicationManager}{Anakin Skywalker}\r\n\\newcommand{\\ApplicationManagerDepartment}{SHS DI DA CEC ITH EH-PLM}\r\n\\newcommand{\\ApplicationManagerContact}{\\href{mailto://anakin.skywalker@siemens-healthineers.com}{anakin.skywalker\\footnotemark[1]}}\r\n\r\n\\newcommand{\\BusinessOwnerName}{Padme Amidala}\r\n\\newcommand{\\BusinessOwnerDepartment}{SHS DI DA CEC EPE}\r\n\\newcommand{\\BusinessOwnerContact}{\\href{mailto://padme.amidala@siemens-healthineers.com}{padme.amidala\\footnotemark[1]}}\r\n\r\n\\newcommand{\\BusinessRepresentativeName}{Luke Skywalker}\r\n\\newcommand{\\BusinessRepresentativeDepartment}{SHS DI DA CEC ITH EH-PLM}\r\n\\newcommand{\\BusinessRepresentativeContact}{\\href{mailto://luke.skywalker@siemens-healthineers.com}{luke.skywalker\\footnotemark[1]}}\r\n\r\n\\newcommand{\\TechnicalContactsNumber}{2}\r\n\\newcommand{\\TechnicalContacts}{\r\n\tObi Wan Kenobi & SHS TE DC SVK D\\&A DIG PTM & \\href{mailto://obi-wan.kenobi@siemens-healthineers.com}{obi-wan.kenobi\\footnotemark[1]} \\\\ &\n\tBaby Yoda & SHS DI D\\&A CEC ITH EH-R\\&D & \\href{mailto://baby.yoda@siemens-healthineers.com}{baby.yoda\\footnotemark[1]} \\\\\r\n}\r\n\r\n% Not needed for Scope document\r\n% Required for Report document\r\n\\newcommand{\\PentestLeadName}{Lukas Nad}\r\n\\newcommand{\\PentestLeadDepartment}{SHS TE DC CYS CSA PPA}\r\n\\newcommand{\\PentestLeadContact}{\\href{mailto://lukas.nad@siemens-healthineers.com}{lukas.nad\\footnotemark[1]}}\r\n\r\n\\newcommand{\\PentestCoordinatorName}{Alzbeta Vojtusova}\r\n\\newcommand{\\PentestCoordinatorDepartment}{SHS TE DC CYS CSA\\& P\\&PA}\r\n\\newcommand{\\PentestCoordinatorContact}{\\href{mailto://alzbeta.vojtusova@siemens-healthineers.com}{alzbeta.vojtusova\\footnotemark[1]}}\r\n\r\n\r\n\r\n\\newcommand{\\PentestParticipantsNumber}{2} % Number of participants in \"Penetration Testing Team\"\r\n\\newcommand{\\PentestTeamMember}{\r\n\tLukas Nad & SHS TE DC CYS CSA P\\&PA & \\href{mailto://lukas.nad@siemens-healthineers.com}{lukas.nad\\footnotemark[1]} \\\\ &\n\tMichal Olencin & SHS TE DC CYS CSA P\\&PA & \\href{mailto://michal.olencin@siemens-healthineers.com}{michal.olencin\\footnotemark[1]} \\\\\r\n}\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tTARGET INFORMATION\r\n%----------------------------------------------------------------------------------------\r\n\\newcommand{\\TargetInfoName}{\\ReportProjectName} %% Asset Name\r\n\\newcommand{\\TargetInfoVersion}{12.1.1.2} %% Asset Version \t\r\n\\newcommand{\\TargetInfoType}{\\AssetType} %% Asset Type\r\n\\newcommand{\\TargetInfoEnvironment}{Testing Environment}\r\n\\newcommand{\\TargetInfoInternetFacing}{Yes} %% Asset Internet Facing\r\n\\newcommand{\\TargetInfoSNXConnectivity}{No} %% SNX Connectivity\r\n\\newcommand{\\TargetInfoHostingLocation}{Special Network} %% Hosting Location\r\n\\newcommand{\\TargetInfoHostingProvider}{N/A} %% Hosting Provider\r\n\\newcommand{\\TargetInfoLifecyclePhase}{Pre-Production}\r\n\\newcommand{\\TargetInfoCriticality}{N/A}\r\n\\newcommand{\\TargetInfoAssetID}{N/A}\r\n\\newcommand{\\TargetInfoSHARPUUID}{N/A} %% SHARP UUID\r\n\\newcommand{\\TargetInfoDescription}{Lorem ipsum dolor sit amet, consectetur adipiscing elit.}\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tAGREED TIMEFRAME\r\n%----------------------------------------------------------------------------------------\r\n\\newcommand{\\TimeframeTotal}{10 working days} \r\n\\newcommand{\\TimeframeStart}{2023-05-29} \r\n\\newcommand{\\TimeframeEnd}{2023-06-09} \r\n\\newcommand{\\TimeframeReportDue}{2023-06-12} \r\n\\newcommand{\\TimeframeComment}{-}\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tFINDINGS COUNT AND OVERALL THREAT EXPOSURE\r\n%----------------------------------------------------------------------------------------\r\n% Not needed for Scope document\r\n% Required for Report document\r\n\r\n\\newcommand{\\OverallThreatExposureImage}{\\includegraphics{Images/CriticalThreat.png}}\r\n% \\includegraphics{Images/CriticalThreat.png}, \r\n% \\includegraphics{Images/HighThreat.png}, \r\n% \\includegraphics{Images/MediumThreat.png}, \r\n% \\includegraphics{Images/LowThreat.png}\r\n\r\n\\newcommand{\\FindingsCountCritical}{0}\r\n\\newcommand{\\FindingsCountHigh}{1}\r\n\\newcommand{\\FindingsCountMedium}{2}\r\n\\newcommand{\\FindingsCountLow}{2}\r\n\\newcommand{\\FindingsCountInfo}{2}\r\n\\newcommand{\\FindingsCountTotal}{7}", resultDecoded);
    }
}