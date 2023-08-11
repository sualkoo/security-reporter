using System.IO.Compression;
using FluentAssertions;
using NUnit.Framework;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services;

namespace webapiTests.ProjectSearch.Services
{
    [TestFixture()]
    [Ignore("")]
    public class TestingMethodologyExtractorTests
    {
        private ZipArchive zipArchive;

        [SetUp()]
        public void SetUp()
        {
            zipArchive = ZipFile.OpenRead("../../../ProjectSearch/ParserTestResources/parserUnitTestsZip.zip");
            Assert.IsNotNull(zipArchive);
        }

        [TearDown()]
        public void TearDown()
        {
            zipArchive.Dispose();
        }

        [Test()]
        public void Empty()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("TestingMethodology/Empty/Executive_Summary.tex");
            TestingMethodologyExtractor tme = new TestingMethodologyExtractor(entry);
            Assert.IsNotNull(tme);

            TestingMethodology parsedMethodology = tme.ExtractTestingMethodology();
            TestingMethodology testMethodology = new TestingMethodology();
            testMethodology.ToolsUsed = new List<Tool>();
            testMethodology.AttackVectors = new List<string>();

            parsedMethodology.Should().BeEquivalentTo(testMethodology);
        }

        [Test()]
        public void EmptyCommands()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("TestingMethodology/EmptyCommands/Attack_Vector_Empty.tex");
            TestingMethodologyExtractor tme = new TestingMethodologyExtractor(entry);
            Assert.IsNotNull(tme);

            Assert.That(() => tme.ExtractTestingMethodology(), Throws.Exception.With.Message.EqualTo("The LaTeX file is not formatted as required"));

            entry = zipArchive.GetEntry("TestingMethodology/EmptyCommands/Attack_Vector_No_Itemize.tex");
            tme = new TestingMethodologyExtractor(entry);
            Assert.That(() => tme.ExtractTestingMethodology(), Throws.Exception.With.Message.EqualTo("The LaTeX file is not formatted as required"));

            entry = zipArchive.GetEntry("TestingMethodology/EmptyCommands/Empty_Tools_Used.tex");
            tme = new TestingMethodologyExtractor(entry);
            TestingMethodology parsedMethodology = tme.ExtractTestingMethodology();
            TestingMethodology testMethodology = new TestingMethodology();
            testMethodology.ToolsUsed = new List<Tool>();
            setAttackVectors(testMethodology);

            parsedMethodology.Should().BeEquivalentTo(testMethodology);

        }

        [Test()]
        public void FullInformation()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("TestingMethodology/FullInformation/Executive_Summary.tex");
            TestingMethodologyExtractor tme = new TestingMethodologyExtractor(entry);
            Assert.IsNotNull(tme);

            TestingMethodology parsedMethodology = tme.ExtractTestingMethodology();
            Assert.IsNotNull(parsedMethodology);

            TestingMethodology testMethodology = new TestingMethodology();


            setToolsUsed(testMethodology);
            setAttackVectors(testMethodology);

            parsedMethodology.Should().BeEquivalentTo(testMethodology);
        }

        [Test()]
        public void MissingCommands()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("TestingMethodology/MissingCommand/Missing_Tools_Used.tex");
            TestingMethodologyExtractor tme = new TestingMethodologyExtractor(entry);
            Assert.IsNotNull(tme);

            TestingMethodology parsedMethodology = tme.ExtractTestingMethodology();
            TestingMethodology testMethdology = new TestingMethodology();
            setAttackVectors(testMethdology);
            testMethdology.ToolsUsed = new List<Tool>();

            parsedMethodology.Should().BeEquivalentTo(testMethdology);

            entry = zipArchive.GetEntry("TestingMethodology/MissingCommand/Missing_Attack_Vector.tex");
            tme = new TestingMethodologyExtractor(entry);
            Assert.IsNotNull(tme);

            parsedMethodology = tme.ExtractTestingMethodology();
            testMethdology = new TestingMethodology();
            setToolsUsed(testMethdology);
            testMethdology.AttackVectors = new List<string>();

            parsedMethodology.Should().BeEquivalentTo(testMethdology);

            entry = zipArchive.GetEntry("TestingMethodology/MissingCommand/Missing_Both.tex");
            tme = new TestingMethodologyExtractor(entry);
            Assert.IsNotNull(tme);

            parsedMethodology = tme.ExtractTestingMethodology();
            testMethdology = new TestingMethodology();
            testMethdology.ToolsUsed = new List<Tool>();
            testMethdology.AttackVectors = new List<string>();

            parsedMethodology.Should().BeEquivalentTo(testMethdology);
        }

        private void setToolsUsed(TestingMethodology testMethodology)
        {
            testMethodology.ToolsUsed = new List<Tool>();
            Tool tool1 = new Tool();
            tool1.ToolName = "adb";
            tool1.Version = "1.0.41";
            tool1.TestType = "Android debugging";
            tool1.WorkType = "Bridge to Andorid device";
            Tool tool2 = new Tool();
            tool2.ToolName = "Android Studio";
            tool2.Version = "2022.2.1";
            tool2.TestType = "Android Development, Emulator";
            tool2.WorkType = "Official integrated development environment for Google's Android operating system, with emulator capabilities.";
            Tool tool3 = new Tool();
            tool3.ToolName = "apktool";
            tool3.Version = "v2.7.0-dirty";
            tool3.TestType = "Reverse Engineering";
            tool3.WorkType = "APK decompiler";

            testMethodology.ToolsUsed.Add(tool1);
            testMethodology.ToolsUsed.Add(tool2);
            testMethodology.ToolsUsed.Add(tool3);
        }

        private void setAttackVectors(TestingMethodology testMethodology)
        {
            testMethodology.AttackVectors = new List<string>
            {
                "static analysis",
                "file system analysis",
                "debugging",
                "workflow analysis",
                "client-side testing",
                "testing for weak cryptography",
                "testing error handling."
            };
        }
    }
}