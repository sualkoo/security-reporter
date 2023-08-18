using System.Collections.Generic;
using System.IO.Compression;
using FluentAssertions;
using NUnit.Framework;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services;

namespace webapiTests.ProjectSearch.Services;

[TestFixture]
public class TestingMethodologyExtractorTests
{
    [SetUp]
    public void SetUp()
    {
        zipArchive = ZipFile.OpenRead("../../../ProjectSearch/ParserTestResources/parserUnitTestsZip.zip");
        Assert.IsNotNull(zipArchive);
    }

    [TearDown]
    public void TearDown()
    {
        zipArchive.Dispose();
    }

    private ZipArchive zipArchive;

    [Test]
    public void Empty()
    {
        var entry = zipArchive.GetEntry("TestingMethodology/Empty/Executive_Summary.tex");
        var tme = new TestingMethodologyExtractor(entry);
        Assert.IsNotNull(tme);

        var parsedMethodology = tme.ExtractTestingMethodology();
        var testMethodology = new TestingMethodology
        {
            ToolsUsed = new List<Tool>(),
            AttackVectors = new List<string>()
        };

        parsedMethodology.Should().BeEquivalentTo(testMethodology);
    }

    [Test]
    public void EmptyCommands()
    {
        var entry = zipArchive.GetEntry("TestingMethodology/EmptyCommands/Attack_Vector_Empty.tex");
        var tme = new TestingMethodologyExtractor(entry);
        Assert.IsNotNull(tme);

        Assert.That(() => tme.ExtractTestingMethodology(),
            Throws.Exception.With.Message.EqualTo("The LaTeX file is not formatted as required"));

        entry = zipArchive.GetEntry("TestingMethodology/EmptyCommands/Attack_Vector_No_Itemize.tex");
        tme = new TestingMethodologyExtractor(entry);
        Assert.That(() => tme.ExtractTestingMethodology(),
            Throws.Exception.With.Message.EqualTo("The LaTeX file is not formatted as required"));

        entry = zipArchive.GetEntry("TestingMethodology/EmptyCommands/Empty_Tools_Used.tex");
        tme = new TestingMethodologyExtractor(entry);
        var parsedMethodology = tme.ExtractTestingMethodology();
        var testMethodology = new TestingMethodology
        {
            ToolsUsed = new List<Tool>()
        };
        SetAttackVectors(testMethodology);

        parsedMethodology.Should().BeEquivalentTo(testMethodology);
    }

    [Test]
    public void FullInformation()
    {
        var entry = zipArchive.GetEntry("TestingMethodology/FullInformation/Executive_Summary.tex");
        var tme = new TestingMethodologyExtractor(entry);
        Assert.IsNotNull(tme);

        var parsedMethodology = tme.ExtractTestingMethodology();
        Assert.IsNotNull(parsedMethodology);

        var testMethodology = new TestingMethodology();


        SetToolsUsed(testMethodology);
        SetAttackVectors(testMethodology);

        parsedMethodology.Should().BeEquivalentTo(testMethodology);
    }

    [Test]
    public void MissingCommands()
    {
        var entry = zipArchive.GetEntry("TestingMethodology/MissingCommand/Missing_Tools_Used.tex");
        var tme = new TestingMethodologyExtractor(entry);
        Assert.IsNotNull(tme);

        var parsedMethodology = tme.ExtractTestingMethodology();
        var testMethdology = new TestingMethodology();
        SetAttackVectors(testMethdology);
        testMethdology.ToolsUsed = new List<Tool>();

        parsedMethodology.Should().BeEquivalentTo(testMethdology);

        entry = zipArchive.GetEntry("TestingMethodology/MissingCommand/Missing_Attack_Vector.tex");
        tme = new TestingMethodologyExtractor(entry);
        Assert.IsNotNull(tme);

        parsedMethodology = tme.ExtractTestingMethodology();
        testMethdology = new TestingMethodology();
        SetToolsUsed(testMethdology);
        testMethdology.AttackVectors = new List<string>();

        parsedMethodology.Should().BeEquivalentTo(testMethdology);

        entry = zipArchive.GetEntry("TestingMethodology/MissingCommand/Missing_Both.tex");
        tme = new TestingMethodologyExtractor(entry);
        Assert.IsNotNull(tme);

        parsedMethodology = tme.ExtractTestingMethodology();
        testMethdology = new TestingMethodology
        {
            ToolsUsed = new List<Tool>(),
            AttackVectors = new List<string>()
        };

        parsedMethodology.Should().BeEquivalentTo(testMethdology);
    }

    private void SetToolsUsed(TestingMethodology testMethodology)
    {
        testMethodology.ToolsUsed = new List<Tool>();
        var tool1 = new Tool
        {
            ToolName = "adb",
            Version = "1.0.41",
            TestType = "Android debugging",
            WorkType = "Bridge to Andorid device"
        };
        var tool2 = new Tool
        {
            ToolName = "Android Studio",
            Version = "2022.2.1",
            TestType = "Android Development, Emulator",
            WorkType = "Official integrated development environment for Google's Android operating system, with emulator capabilities."
        };
        var tool3 = new Tool();
        tool3.ToolName = "apktool";
        tool3.Version = "v2.7.0-dirty";
        tool3.TestType = "Reverse Engineering";
        tool3.WorkType = "APK decompiler";

        testMethodology.ToolsUsed.Add(tool1);
        testMethodology.ToolsUsed.Add(tool2);
        testMethodology.ToolsUsed.Add(tool3);
    }

    private void SetAttackVectors(TestingMethodology testMethodology)
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