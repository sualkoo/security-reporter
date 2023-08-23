using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.ProjectSearch.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

namespace webapiTests.ProjectSearch.Services
{
    [TestFixture]
    public class ProjectInformationExtractorTests
    {
        private ZipArchive zipArchive;

        [SetUp] 
        public void SetUp() {
            zipArchive = ZipFile.OpenRead("../../../ProjectSearch/ParserTestResources/parserUnitTestsZip.zip");
            Assert.IsNotNull(zipArchive);
        }

        [TearDown] 
        public void TearDown()
        {
            zipArchive?.Dispose();
        }

        [Test] public void FullInformation()
        {
            ZipArchiveEntry pentestTeamEntry = zipArchive.GetEntry("ProjectInformation/PentestTeam.tex");
            Assert.IsNotNull (pentestTeamEntry);
            PentestTeamExtractor pentestTeam = new PentestTeamExtractor(pentestTeamEntry);
            Dictionary<string, ProjectInformationParticipant> pentestTeamMembers = pentestTeam.ExtractPentestTeam();
            ZipArchiveEntry entry = zipArchive.GetEntry("ProjectInformation/FullInformation/Project_Information.tex");
            Assert.IsNotNull(entry);

            ProjectInformation parsedProjectInformation = ProjectInformationExtractor.ExtractProjectInformation(entry, pentestTeamMembers);
            ProjectInformation expectedProjectInformation = new ProjectInformation();

            ProjectInformationParticipant applicationManager = new ProjectInformationParticipant();
            applicationManager.Name = "Anakin Skywalker";
            applicationManager.Department = "SHS DI D\\&A CEC ITH EH-PLM";
            applicationManager.Contact = "anakin.skywalker@siemens-healthineers.com";

            expectedProjectInformation.ApplicationManager = applicationManager;

            ProjectInformationParticipant businessOwner = new ProjectInformationParticipant();
            businessOwner.Name = "Padme Amidala";
            businessOwner.Department = "SHS DI D\\&A CEC EPE";
            businessOwner.Contact = "padme.amidala@siemens-healthineers.com";
            expectedProjectInformation.BusinessOwner = businessOwner;


            ProjectInformationParticipant businessRepresentative = new ProjectInformationParticipant();
            businessRepresentative.Name = "Luke Skywalker";
            businessRepresentative.Department = "SHS DI D\\&A CEC ITH EH-PLM";
            businessRepresentative.Contact = "luke.skywalker@siemens-healthineers.com";
            expectedProjectInformation.BusinessRepresentative = businessRepresentative;


            ProjectInformationParticipant technicalContact1 = new ProjectInformationParticipant();
            technicalContact1.Name = "Obi Wan Kenobi";
            technicalContact1.Department = "SHS TE DC SVK D\\&A DIG PTM";
            technicalContact1.Contact = "obi-wan.kenobi@siemens-healthineers.com";

            ProjectInformationParticipant technicalContact2 = new ProjectInformationParticipant();
            technicalContact2.Name = "Baby Yoda";
            technicalContact2.Department = " SHS DI D\\&A CEC ITH EH-R\\&D";
            technicalContact2.Contact = "baby.yoda@siemens-healthineers.com";

            expectedProjectInformation.TechnicalContacts.Add(technicalContact1);
            expectedProjectInformation.TechnicalContacts.Add(technicalContact2);


            ProjectInformationParticipant pentestLead = new ProjectInformationParticipant();
            pentestLead.Name = "Lukas Nad";
            pentestLead.Department = "SHS TE DC CYS CSA P\\&PA";
            pentestLead.Contact = "lukas.nad@siemens-healthineers.com";
            expectedProjectInformation.PentestLead = pentestLead;

            ProjectInformationParticipant pentestCoordinator = new ProjectInformationParticipant();
            pentestCoordinator.Name = "Alzbeta Vojtusova";
            pentestCoordinator.Department = "SHS TE DC CYS CSA P\\&PA";
            pentestCoordinator.Contact = "alzbeta.vojtusova@siemens-healthineers.com";
            expectedProjectInformation.PentestCoordinator = pentestCoordinator;

            ProjectInformationParticipant pentestTeamMember1 = new ProjectInformationParticipant();
            pentestTeamMember1.Name = "Lukas Nad";
            pentestTeamMember1.Department = "SHS TE DC CYS CSA P\\&PA";
            pentestTeamMember1.Contact = "lukas.nad@siemens-healthineers.com";

            ProjectInformationParticipant pentestTeamMember2 = new ProjectInformationParticipant();
            pentestTeamMember2.Name = "Michal Olencin";
            pentestTeamMember2.Department = "SHS TE DC CYS CSA P\\&PA";
            pentestTeamMember2.Contact = "michal.olencin@siemens-healthineers.com";

            ProjectInformationParticipant pentestTeamMember3 = new ProjectInformationParticipant();
            pentestTeamMember3.Name = "Medhavi Taksh";
            pentestTeamMember3.Department = "SHS TE DC CYS LAB";
            pentestTeamMember3.Contact = "taksh.bhatt@siemens-healthineers.com";
            expectedProjectInformation.PentestTeam.Add(pentestTeamMember1);
            expectedProjectInformation.PentestTeam.Add(pentestTeamMember2);
            expectedProjectInformation.PentestTeam.Add(pentestTeamMember3);

            expectedProjectInformation.TargetInfoVersion = "12.1.1.2";
            expectedProjectInformation.TargetInfoEnvironment = "Testing Environemnt";
            expectedProjectInformation.TargetInfoInternetFacing = true;
            expectedProjectInformation.TargetInfoSNXConnectivity = false;
            expectedProjectInformation.TargetInfoHostingLocation = "Special Network";
            expectedProjectInformation.TargetInfoHostingProvider = "N/A";
            expectedProjectInformation.TargetInfoLifeCyclePhase = "Pre-Production";
            expectedProjectInformation.TargetInfoCriticality = "N/A";
            expectedProjectInformation.TargetInfoAssetID = "N/A";
            expectedProjectInformation.TargetInfoSHARPUUID = "N/A";
            expectedProjectInformation.TargetInfoDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pharetra pretium. Cras varius purus eu cursus vehicula. Sed in molestie arcu, id placerat velit. Praesent sagittis purus in neque convallis, a faucibus odio egestas. Nam ultrices, metus et mattis facilisis, felis lectus tempor velit, a interdum nisl libero nec dui. Mauris interdum scelerisque semper. Cras mattis id lacus a ullamcorper. Curabitur fermentum vehicula leo, vel convallis turpis luctus nec. In mollis vitae diam in ornare. Donec molestie augue nisl, malesuada maximus urna gravida quis. Curabitur ac ante turpis. Nulla facilisi. Aenean eleifend ipsum at velit lobortis, in hendrerit arcu dapibus. Proin ut lacus sed tellus maximus euismod. Suspendisse elementum mauris tellus, eget imperdiet leo dictum nec. Fusce tortor mauris, iaculis non tristique ut, condimentum a odio.";
            expectedProjectInformation.TimeFrameTotal = 10;
            expectedProjectInformation.TimeFrameStart = new DateTime(2023, 5, 29);
            expectedProjectInformation.TimeFrameEnd = new DateTime(2023, 6, 9);
            expectedProjectInformation.TimeFrameReportDue = new DateTime(2023, 6, 12);
            expectedProjectInformation.TimeFrameComment = "-";
            expectedProjectInformation.FindingsCountCritical = 0;
            expectedProjectInformation.FindingsCountHigh = 1;
            expectedProjectInformation.FindingsCountMedium = 2;
            expectedProjectInformation.FindingsCountLow = 2;
            expectedProjectInformation.FindingsCountInfo = 2;
            expectedProjectInformation.FindingsCountTotal = 7;

            parsedProjectInformation.Should().BeEquivalentTo(expectedProjectInformation);
        }
    }
}
