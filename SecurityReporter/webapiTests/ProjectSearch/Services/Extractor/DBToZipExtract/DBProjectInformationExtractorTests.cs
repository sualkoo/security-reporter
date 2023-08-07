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
        var extractor = new DbProjectInformationExtractor();

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
        
        // Contains desired result
        StringAssert.Contains("%----------------------------------------------------------------------------------------\n%\tPROJECT INFORMATION\n%----------------------------------------------------------------------------------------\n\\newcommand{\\ApplicationManager}{Anakin Skywalker}\n\\newcommand{\\ApplicationManagerDepartment}{SHS DI DA CEC ITH EH-PLM}\n\\newcommand{\\ApplicationManagerContact}{\\href{mailto://anakin.skywalker@siemens-healthineers.com}{anakin.skywalker\\footnotemark[1]}}\n\n\\newcommand{\\BusinessOwnerName}{Padme Amidala}\n\\newcommand{\\BusinessOwnerDepartment}{SHS DI DA CEC EPE}\n\\newcommand{\\BusinessOwnerContact}{\\href{mailto://padme.amidala@siemens-healthineers.com}{padme.amidala\\footnotemark[1]}}\n\n\\newcommand{\\BusinessRepresentativeName}{Luke Skywalker}\n\\newcommand{\\BusinessRepresentativeDepartment}{SHS DI DA CEC ITH EH-PLM}\n\\newcommand{\\BusinessRepresentativeContact}{\\href{mailto://luke.skywalker@siemens-healthineers.com}{luke.skywalker\\footnotemark[1]}}\n\n\\newcommand{\\TechnicalContactsNumber}{2}\n\\newcommand{\\TechnicalContacts}{\n\tObi Wan Kenobi & SHS TE DC SVK D\\&A DIG PTM & \\href{mailto://obi-wan.kenobi@siemens-healthineers.com}{obi-wan.kenobi\\footnotemark[1]} \\\\ &\n\tBaby Yoda & SHS DI D\\&A CEC ITH EH-R\\&D & \\href{mailto://baby.yoda@siemens-healthineers.com}{baby.yoda\\footnotemark[1]} \\\\\n}\n\n% Not needed for Scope document\n% Required for Report document\n\\newcommand{\\PentestLeadName}{Lukas Nad}\n\\newcommand{\\PentestLeadDepartment}{SHS TE DC CYS CSA PPA}\n\\newcommand{\\PentestLeadContact}{\\href{mailto://lukas.nad@siemens-healthineers.com}{lukas.nad\\footnotemark[1]}}\n\n\\newcommand{\\PentestCoordinatorName}{Alzbeta Vojtusova}\n\\newcommand{\\PentestCoordinatorDepartment}{SHS TE DC CYS CSA\\& P\\&PA}\n\\newcommand{\\PentestCoordinatorContact}{\\href{mailto://alzbeta.vojtusova@siemens-healthineers.com}{alzbeta.vojtusova\\footnotemark[1]}}\n\n\n\n\\newcommand{\\PentestParticipantsNumber}{2} % Number of participants in \"Penetration Testing Team\"\n\\newcommand{\\PentestTeamMember}{\n\tLukas Nad & SHS TE DC CYS CSA P\\&PA & \\href{mailto://lukas.nad@siemens-healthineers.com}{lukas.nad\\footnotemark[1]} \\\\ &\n\tMichal Olencin & SHS TE DC CYS CSA P\\&PA & \\href{mailto://michal.olencin@siemens-healthineers.com}{michal.olencin\\footnotemark[1]} \\\\\n}\n\n%----------------------------------------------------------------------------------------\n%\tTARGET INFORMATION\n%----------------------------------------------------------------------------------------\n\\newcommand{\\TargetInfoName}{\\ReportProjectName} %% Asset Name\n\\newcommand{\\TargetInfoVersion}{12.1.1.2} %% Asset Version \t\n\\newcommand{\\TargetInfoType}{\\AssetType} %% Asset Type\n\\newcommand{\\TargetInfoEnvironment}{Testing Environment}\n\\newcommand{\\TargetInfoInternetFacing}{Yes} %% Asset Internet Facing\n\\newcommand{\\TargetInfoSNXConnectivity}{No} %% SNX Connectivity\n\\newcommand{\\TargetInfoHostingLocation}{Special Network} %% Hosting Location\n\\newcommand{\\TargetInfoHostingProvider}{N/A} %% Hosting Provider\n\\newcommand{\\TargetInfoLifecyclePhase}{Pre-Production}\n\\newcommand{\\TargetInfoCriticality}{N/A}\n\\newcommand{\\TargetInfoAssetID}{N/A}\n\\newcommand{\\TargetInfoSHARPUUID}{N/A} %% SHARP UUID\n\\newcommand{\\TargetInfoDescription}{Lorem ipsum dolor sit amet, consectetur adipiscing elit.}\n\n%----------------------------------------------------------------------------------------\n%\tAGREED TIMEFRAME\n%----------------------------------------------------------------------------------------\n\\newcommand{\\TimeframeTotal}{10 working days} \n\\newcommand{\\TimeframeStart}{2023-05-29} \n\\newcommand{\\TimeframeEnd}{2023-06-09} \n\\newcommand{\\TimeframeReportDue}{2023-06-12} \n\\newcommand{\\TimeframeComment}{-}\n\n%----------------------------------------------------------------------------------------\n%\tFINDINGS COUNT AND OVERALL THREAT EXPOSURE\n%----------------------------------------------------------------------------------------\n% Not needed for Scope document\n% Required for Report document\n\n\\newcommand{\\OverallThreatExposureImage}{\\includegraphics{Images/CriticalThreat.png}}\n% \\includegraphics{Images/CriticalThreat.png}, \n% \\includegraphics{Images/HighThreat.png}, \n% \\includegraphics{Images/MediumThreat.png}, \n% \\includegraphics{Images/LowThreat.png}\n\n\\newcommand{\\FindingsCountCritical}{0}\n\\newcommand{\\FindingsCountHigh}{1}\n\\newcommand{\\FindingsCountMedium}{2}\n\\newcommand{\\FindingsCountLow}{2}\n\\newcommand{\\FindingsCountInfo}{2}\n\\newcommand{\\FindingsCountTotal}{7}", resultDecoded);
    }
}