using System.Collections.Generic;
using System.IO.Compression;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using webapi.ProjectSearch.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

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
        var testMethodology = new TestingMethodology();

        parsedMethodology.Should().BeEquivalentTo(testMethodology);
    }

    [Test]
    public void EmptyCommands()
    {
        var entry = zipArchive.GetEntry("TestingMethodology/EmptyCommands/Attack_Vector_Empty.tex");
        var tme = new TestingMethodologyExtractor(entry);
        Assert.IsNotNull(tme);
        TestingMethodology testMethodology = new TestingMethodology();
        SetToolsUsed(testMethodology);
        testMethodology.AttackVectors = @"
	Tests on \ReportProjectName included, but were not limited to:


";
        var parsedMethodology = tme.ExtractTestingMethodology();

        compareStringAttributes(parsedMethodology.ToolsUsed, testMethodology.ToolsUsed);
        compareStringAttributes(parsedMethodology.AttackVectors, testMethodology.AttackVectors);

        entry = zipArchive.GetEntry("TestingMethodology/EmptyCommands/Attack_Vector_No_Itemize.tex");
        tme = new TestingMethodologyExtractor(entry);
        parsedMethodology = tme.ExtractTestingMethodology();

        testMethodology.AttackVectors = @"
	Tests on \ReportProjectName included, but were not limited to:
		\item static analysis,
		\item file system analysis,
		\item debugging,
		\item workflow analysis,
		\item client-side testing,
		\item testing for weak cryptography,
		\item testing error handling.";
        SetToolsUsed(testMethodology);

        compareStringAttributes(parsedMethodology.ToolsUsed, testMethodology.ToolsUsed);
        compareStringAttributes(parsedMethodology.AttackVectors, testMethodology.AttackVectors);

        entry = zipArchive.GetEntry("TestingMethodology/EmptyCommands/Empty_Tools_Used.tex");
        tme = new TestingMethodologyExtractor(entry);
        parsedMethodology = tme.ExtractTestingMethodology();
        testMethodology = new TestingMethodology();
        SetAttackVectors(testMethodology);


        compareStringAttributes(parsedMethodology.ToolsUsed, testMethodology.ToolsUsed);
        compareStringAttributes(parsedMethodology.AttackVectors, testMethodology.AttackVectors);
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

        compareStringAttributes(parsedMethodology.ToolsUsed, testMethodology.ToolsUsed);
        compareStringAttributes(parsedMethodology.AttackVectors, testMethodology.AttackVectors);
    }

    [Test]
    public void MissingCommands()
    {
        var entry = zipArchive.GetEntry("TestingMethodology/MissingCommand/Missing_Tools_Used.tex");
        var tme = new TestingMethodologyExtractor(entry);
        Assert.IsNotNull(tme);

        var parsedMethodology = tme.ExtractTestingMethodology();
        var testMethodology = new TestingMethodology();
        SetAttackVectors(testMethodology); ;


        //testMethdology.ToolsUsed = new List<Tool>();

        compareStringAttributes(parsedMethodology.ToolsUsed, testMethodology.ToolsUsed);
        compareStringAttributes(parsedMethodology.AttackVectors, testMethodology.AttackVectors);

        entry = zipArchive.GetEntry("TestingMethodology/MissingCommand/Missing_Attack_Vector.tex");
        tme = new TestingMethodologyExtractor(entry);
        Assert.IsNotNull(tme);

        parsedMethodology = tme.ExtractTestingMethodology();
        testMethodology = new TestingMethodology();
        SetToolsUsed(testMethodology);
        //testMethdology.AttackVectors = new List<string>();

        compareStringAttributes(parsedMethodology.ToolsUsed, testMethodology.ToolsUsed);
        compareStringAttributes(parsedMethodology.AttackVectors, testMethodology.AttackVectors);

        entry = zipArchive.GetEntry("TestingMethodology/MissingCommand/Missing_Both.tex");
        tme = new TestingMethodologyExtractor(entry);
        Assert.IsNotNull(tme);

        parsedMethodology = tme.ExtractTestingMethodology();

        testMethodology = new TestingMethodology();

        parsedMethodology.Should().BeEquivalentTo(testMethodology);
    }

    private void SetToolsUsed(TestingMethodology testMethodology)
    {
        testMethodology.ToolsUsed = @"
	\hline
	adb			& 1.0.41			&	Android debugging	&	Bridge to Andorid device\\
	\hline
	Android Studio		& 2022.2.1			&	Android Development, Emulator	&	Official integrated development environment for Google's Android operating system, with emulator capabilities. \\
	\hline
	apktool			& v2.7.0-dirty			&	Reverse Engineering	&	APK decompiler\\
	\hline
"; 
    }

    private void SetAttackVectors(TestingMethodology testMethodology)
    {
        testMethodology.AttackVectors = @"

	Tests on \ReportProjectName included, but were not limited to:

	\begin{itemize}
		\item static analysis,
		\item file system analysis,
		\item debugging,
		\item workflow analysis,
		\item client-side testing,
		\item testing for weak cryptography,
		\item testing error handling.
	\end{itemize}

";
    }

    private void compareStringAttributes(string str1, string str2)
    {
        var str1Join = string.Join("",
                str1.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();

        var str2Join = string.Join("",
                str2.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();

        str1Join.Should().Be(str2Join);
    }
}