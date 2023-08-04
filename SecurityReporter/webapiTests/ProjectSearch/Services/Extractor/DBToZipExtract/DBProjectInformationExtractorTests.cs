using NUnit.Framework;
using System.Text;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract.Tests
{
    [TestFixture()]
    public class DBProjectInformationExtractorTests
    {
        [Test()]
        public void extractProjectInformationTest()
        {
            // Arrange
            DBProjectInformationExtractor extractor = new DBProjectInformationExtractor();

            ProjectInformation projectInformation = new ProjectInformation();
            projectInformation.ApplicationManager = new ProjectInformationParticipant()
            {
                Name = "Anakin Skywalker",
                Department = "SHS DI DA CEC ITH EH-PLM",
                Contact = "anakin.skywalker@siemens-healthineers.com"
            };
            projectInformation.BusinessOwner = new ProjectInformationParticipant()
            {
                Name = "Padme Amidala",
                Department = "SHS DI DA CEC EPE",
                Contact = "padme.amidala@siemens-healthineers.com"
            };
            projectInformation.BusinessRepresentative = new ProjectInformationParticipant()
            {
                Name = "Luke Skywalker",
                Department = "SHS DI DA CEC ITH EH-PLM",
                Contact = "luke.skywalker@siemens-healthineers.com"
            };
            projectInformation.TechnicalContacts = new List<ProjectInformationParticipant>();
            ProjectInformationParticipant technicalContact = new ProjectInformationParticipant() 
            {
                Name = "Obi Wan Kenobi",
                Department = "SHS TE DC SVK D&A DIG PTM",
                Contact = "obi-wan.kenobi@siemens-healthineers.com"
            };
            ProjectInformationParticipant technicalContact2 = new ProjectInformationParticipant()
            {
                Name = "Baby Yoda",
                Department = "SHS DI D&A CEC ITH EH-R&D",
                Contact = "baby.yoda@siemens-healthineers.com"
            };
            projectInformation.TechnicalContacts.Add(technicalContact);
            projectInformation.TechnicalContacts.Add(technicalContact2);

            ProjectInformationParticipant pentestLead = new ProjectInformationParticipant()
            {
                Name = "Lukas Nad",
                Department = "SHS TE DC CYS CSA PPA",
                Contact = "lukas.nad@siemens-healthineers.com"
            };
            projectInformation.PentestLead = pentestLead;
            ProjectInformationParticipant pentestCoordinator = new ProjectInformationParticipant()
            {
                Name = "Alzbeta Vojtusova",
                Department = "SHS TE DC CYS CSA& P&PA",
                Contact = "alzbeta.vojtusova@siemens-healthineers.com"
            };
            projectInformation.PentestCoordinator = pentestCoordinator;
            projectInformation.PentestTeam = new List<ProjectInformationParticipant>();
            ProjectInformationParticipant PentestTeamParticipant = new ProjectInformationParticipant()
            {
                Name = "Lukas Nad",
                Department = "SHS TE DC CYS CSA P&PA",
                Contact = "lukas.nad@siemens-healthineers.com"
            };
            ProjectInformationParticipant PentestTeamParticipant2 = new ProjectInformationParticipant()
            {
                Name = "Michal Olencin",
                Department = "SHS TE DC CYS CSA P&PA",
                Contact = "michal.olencin@siemens-healthineers.com"
            };
            projectInformation.PentestTeam.Add(PentestTeamParticipant);
            projectInformation.PentestTeam.Add(PentestTeamParticipant2);

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
            var result = extractor.extractProjectInformation(projectInformation);
            var resultDecoded = Encoding.UTF8.GetString(result);

            // Assert
            Assert.IsNotNull(result);
            // Application Manager
            StringAssert.Contains("\\newcommand{\\ApplicationManager}{Anakin Skywalker}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\ApplicationManagerDepartment}{SHS DI D\\&A CEC ITH EH-PLM}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\ApplicationManagerContact}{\\href{mailto://anakin.skywalker@siemens-healthineers.com}{anakin.skywalker\\footnotemark[1]}}", resultDecoded);
            // Business Owner
            StringAssert.Contains("\\newcommand{\\BusinessOwnerName}{Padme Amidala}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\BusinessOwnerDepartment}{SHS DI D\\&A CEC EPE}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\BusinessOwnerContact}{\\href{mailto://padme.amidala@siemens-healthineers.com}{padme.amidala\\footnotemark[1]}}", resultDecoded);
            // Business Representative
            StringAssert.Contains("\\newcommand{\\BusinessRepresentativeName}{Luke Skywalker}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\BusinessRepresentativeDepartment}{SHS DI D\\&A CEC ITH EH-PLM}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\BusinessRepresentativeContact}{\\href{mailto://luke.skywalker@siemens-healthineers.com}{luke.skywalker\\footnotemark[1]}}", resultDecoded);
            // Technical contacts
            StringAssert.Contains("\\newcommand{\\TechnicalContactsNumber}{2}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\TechnicalContacts}{", resultDecoded);
            StringAssert.Contains("\tObi Wan Kenobi & SHS TE DC SVK D\\&A DIG PTM & \\href{obi-wan.kenobi@siemens-healthineers.com}{obi-wan.kenobi\\footnotemark[1]} \\\\ &", resultDecoded);
            StringAssert.Contains("\tBaby Yoda & SHS DI D\\&A CEC ITH EH-R\\&D & \\href{baby.yoda@siemens-healthineers.com}{baby.yoda\\footnotemark[1]} \\\\", resultDecoded);
            StringAssert.Contains("}\r\n\r\n% Not needed for Scope document\r\n% Required for Report document", resultDecoded);
            // Pentest Lead
            StringAssert.Contains("\\newcommand{\\PentestLeadName}{Lukas Nad}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\PentestLeadDepartment}{SHS TE DC CYS CSA P\\&PA}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\PentestLeadContact}{\\href{mailto://lukas.nad@siemens-healthineers.com}{lukas.nad\\footnotemark[1]}}", resultDecoded);
            // Pentest Coordinator
            StringAssert.Contains("\\newcommand{\\PentestCoordinatorName}{Alzbeta Vojtusova}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\PentestCoordinatorDepartment}{SHS TE DC CYS CSA P\\&PA}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\PentestCoordinatorContact}{\\href{mailto://alzbeta.vojtusova@siemens-healthineers.com}{alzbeta.vojtusova\\footnotemark[1]}}", resultDecoded);
            // Pentest Participants
            StringAssert.Contains("\\newcommand{\\PentestParticipantsNumber}{2} % Number of participants in \"Penetration Testing Team\"", resultDecoded);
            StringAssert.Contains("\\newcommand{\\PentestTeamMember}{", resultDecoded);
            StringAssert.Contains("\tLukas Nad & SHS TE DC CYS CSA P\\&PA & \\href{lukas.nad@siemens-healthineers.com}{lukas.nad\\footnotemark[1]} \\\\ &", resultDecoded);
            StringAssert.Contains("\tMichal Olencin & SHS TE DC CYS CSA P&PA & \\href{michal.olencin@siemens-healthineers.com}{michal.olencin\\footnotemark[1]} \\\\", resultDecoded);
            // Closing bracked of previous, Target Information
            StringAssert.Contains("}\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tTARGET INFORMATION\r\n%----------------------------------------------------------------------------------------", resultDecoded);
            StringAssert.Contains("\\newcommand{\\TargetInfoName}{\\ReportProjectName} %% Asset Name\r\n\\newcommand{\\TargetInfoVersion}{12.1.1.2} %% Asset Version \t\r\n\\newcommand{\\TargetInfoType}{\\AssetType} %% Asset Type\r\n\\newcommand{\\TargetInfoEnvironment}{Testing Environment}\r\n\\newcommand{\\TargetInfoInternetFacing}{Yes} %% Asset Internet Facing\r\n\\newcommand{\\TargetInfoSNXConnectivity}{No} %% SNX Connectivity\r\n\\newcommand{\\TargetInfoHostingLocation}{Special Network} %% Hosting Location\r\n\\newcommand{\\TargetInfoHostingProvider}{N/A} %% Hosting Provider\r\n\\newcommand{\\TargetInfoLifecyclePhase}{Pre-Production}\r\n\\newcommand{\\TargetInfoCriticality}{N/A}\r\n\\newcommand{\\TargetInfoAssetID}{N/A}\r\n\\newcommand{\\TargetInfoSHARPUUID}{N/A} %% SHARP UUID\r\n\\newcommand{\\TargetInfoDescription}{Lorem ipsum dolor sit amet, consectetur adipiscing elit.}", resultDecoded);
            // Agreed Timeframe
            StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tAGREED TIMEFRAME\r\n%----------------------------------------------------------------------------------------", resultDecoded);
            StringAssert.Contains("\\newcommand{\\TimeframeTotal}{10 working days} \r\n\\newcommand{\\TimeframeStart}{2023-05-29} \r\n\\newcommand{\\TimeframeEnd}{2023-06-09} \r\n\\newcommand{\\TimeframeReportDue}{2023-06-12} \r\n\\newcommand{\\TimeframeComment}{-}", resultDecoded);
            // Findings Count And Overall Threat Exposure
            StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tFINDINGS COUNT AND OVERALL THREAT EXPOSURE\r\n%----------------------------------------------------------------------------------------\r\n% Not needed for Scope document\r\n% Required for Report document", resultDecoded);
            StringAssert.Contains("\\newcommand{\\OverallThreatExposureImage}{\\includegraphics{Images/CriticalThreat.png}}", resultDecoded);
            StringAssert.Contains("% \\includegraphics{Images/CriticalThreat.png}, \r\n% \\includegraphics{Images/HighThreat.png}, \r\n% \\includegraphics{Images/MediumThreat.png}, \r\n% \\includegraphics{Images/LowThreat.png}", resultDecoded);
            StringAssert.Contains("\\newcommand{\\FindingsCountCritical}{0}\r\n\\newcommand{\\FindingsCountHigh}{1}\r\n\\newcommand{\\FindingsCountMedium}{2}\r\n\\newcommand{\\FindingsCountLow}{2}\r\n\\newcommand{\\FindingsCountInfo}{2}\r\n\\newcommand{\\FindingsCountTotal}{7}", resultDecoded);
        }
    }
}