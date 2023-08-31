using System.IO.Compression;
using FluentAssertions;
using NUnit.Framework;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

namespace webapiTests.ProjectSearch.Services;

[TestFixture]
public class ExecutiveSummaryExtractorTests
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
        var entry = zipArchive.GetEntry("ExecutiveSummary/Empty/Executive_Summary.tex");
        var ese = new ExecutiveSummaryExtractor(entry);
        Assert.IsNotNull(ese);

        var passedSummary = ese.ExtractExecutiveSummary();
        const string testSummary = "";
        Assert.AreEqual(passedSummary, testSummary);
    }

    [Test]
    public void EmptySummary()
    {
        var entry = zipArchive.GetEntry("ExecutiveSummary/EmptySummary/Executive_Summary.tex");
        var ese = new ExecutiveSummaryExtractor(entry);
        Assert.IsNotNull(ese);

        var parsedSummary = ese.ExtractExecutiveSummary();
        const string testSummary = @"%----------------------------------------------------------------------------------------
%	EXECUTIVE SUMMARY
%----------------------------------------------------------------------------------------
%-<ExecSum>->


%-<ExecSum>
\pagebreak
\section*{Overall Exposure}";
        var parSumFormatted = string.Join("",
                parsedSummary.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();
        var testSumFormatted = string.Join("",
                testSummary.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();

        parSumFormatted.Should().Be(testSumFormatted);
    }

    [Test]
    public void FullInformation()
    {
        var entry = zipArchive.GetEntry("ExecutiveSummary/FullInformation/Executive_Summary.tex");
        var ese = new ExecutiveSummaryExtractor(entry);
        Assert.IsNotNull(ese);

        var parsedSummary = ese.ExtractExecutiveSummary();
        const string testSummary = @"%----------------------------------------------------------------------------------------
%	EXECUTIVE SUMMARY
%----------------------------------------------------------------------------------------
%-<ExecSum>->

The Penetration Testing team at SHS TE DC CYS CSA in Slovakia conducted a penetration test of 
\textbf{\textsc{\ReportProjectName}} system in order to assess its overall security posture. 

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pharetra pretium. Cras varius purus eu cursus vehicula. Sed in molestie arcu, id placerat velit. Praesent sagittis purus in neque convallis, a faucibus odio egestas. Nam ultrices, metus et mattis facilisis, felis lectus tempor velit, a interdum nisl libero nec dui. Mauris interdum scelerisque semper. Cras mattis id lacus a ullamcorper. Curabitur fermentum vehicula leo, vel convallis turpis luctus nec. In mollis vitae diam in ornare. Donec molestie augue nisl, malesuada maximus urna gravida quis. Curabitur ac ante turpis. Nulla facilisi. Aenean eleifend ipsum at velit lobortis, in hendrerit arcu dapibus. Proin ut lacus sed tellus maximus euismod. Suspendisse elementum mauris tellus, eget imperdiet leo dictum nec. Fusce tortor mauris, iaculis non tristique ut, condimentum a odio.

Maecenas tincidunt sollicitudin metus id eleifend. Cras justo urna, tempus et mi vestibulum, iaculis pellentesque nunc. Etiam nisi nibh, bibendum sed augue in, molestie lacinia turpis. Ut bibendum pretium mi vel volutpat. Praesent mattis scelerisque neque a vehicula. Cras nec iaculis mi, in rutrum ligula. Suspendisse potenti.

Fusce mollis, erat eget tempus ornare, erat nisl mattis dolor, sed porta mauris quam eget tortor. Etiam bibendum sodales lorem ut fringilla. Phasellus in urna ex. In venenatis turpis a augue egestas efficitur. Interdum et malesuada fames ac ante ipsum primis in faucibus. Cras laoreet odio eu auctor molestie. In congue malesuada sollicitudin.

Integer egestas mollis ex quis semper. Nam vitae diam aliquet, elementum leo molestie, suscipit mauris. Suspendisse ex magna, fermentum eu sem non, convallis faucibus nisl. In id dignissim orci, non sodales ante. Nulla bibendum sem nec turpis porta pulvinar. Curabitur fringilla libero ut ex faucibus, non ultrices nisl volutpat. Quisque imperdiet condimentum diam eu scelerisque. Integer ullamcorper euismod accumsan. Curabitur efficitur, neque non blandit tempus, quam erat viverra risus, eu placerat risus elit eu neque. 

%-<ExecSum>
\pagebreak
\section*{Overall Exposure}";
        var parSumFormatted = string.Join("",
                parsedSummary.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();
        var testSumFormatted = string.Join("",
                testSummary.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();

        parSumFormatted.Should().Be(testSumFormatted);
    }

    [Test]
    public void MissingExecSumComments()
    {
        var entry = zipArchive.GetEntry("ExecutiveSummary/NoExecSum/Executive_Summary.tex");
        var ese = new ExecutiveSummaryExtractor(entry);
        Assert.IsNotNull(ese);

        var parsedSummary = ese.ExtractExecutiveSummary();
        var testSummary = @"%----------------------------------------------------------------------------------------
%	EXECUTIVE SUMMARY
%----------------------------------------------------------------------------------------
\pagebreak
\section*{Overall Exposure}";
        var parSumFormatted = string.Join("",
                parsedSummary.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();
        var testSumFormatted = string.Join("",
                testSummary.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();

        parSumFormatted.Should().Be(testSumFormatted);
    }

    [Test]
    public void SummaryWithNoExecSumComments()
    {
        var entry = zipArchive.GetEntry("ExecutiveSummary/SummaryWithNoExecSum/Executive_Summary.tex");
        var ese = new ExecutiveSummaryExtractor(entry);
        Assert.IsNotNull(ese);

        var parsedSummary = ese.ExtractExecutiveSummary();
        var testSummary = @"%----------------------------------------------------------------------------------------
%	EXECUTIVE SUMMARY
%----------------------------------------------------------------------------------------

The Penetration Testing team at SHS TE DC CYS CSA in Slovakia conducted a penetration test of 
\textbf{\textsc{\ReportProjectName}} system in order to assess its overall security posture. 

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pharetra pretium. Cras varius purus eu cursus vehicula. Sed in molestie arcu, id placerat velit. Praesent sagittis purus in neque convallis, a faucibus odio egestas. Nam ultrices, metus et mattis facilisis, felis lectus tempor velit, a interdum nisl libero nec dui. Mauris interdum scelerisque semper. Cras mattis id lacus a ullamcorper. Curabitur fermentum vehicula leo, vel convallis turpis luctus nec. In mollis vitae diam in ornare. Donec molestie augue nisl, malesuada maximus urna gravida quis. Curabitur ac ante turpis. Nulla facilisi. Aenean eleifend ipsum at velit lobortis, in hendrerit arcu dapibus. Proin ut lacus sed tellus maximus euismod. Suspendisse elementum mauris tellus, eget imperdiet leo dictum nec. Fusce tortor mauris, iaculis non tristique ut, condimentum a odio.

Maecenas tincidunt sollicitudin metus id eleifend. Cras justo urna, tempus et mi vestibulum, iaculis pellentesque nunc. Etiam nisi nibh, bibendum sed augue in, molestie lacinia turpis. Ut bibendum pretium mi vel volutpat. Praesent mattis scelerisque neque a vehicula. Cras nec iaculis mi, in rutrum ligula. Suspendisse potenti.

Fusce mollis, erat eget tempus ornare, erat nisl mattis dolor, sed porta mauris quam eget tortor. Etiam bibendum sodales lorem ut fringilla. Phasellus in urna ex. In venenatis turpis a augue egestas efficitur. Interdum et malesuada fames ac ante ipsum primis in faucibus. Cras laoreet odio eu auctor molestie. In congue malesuada sollicitudin.

Integer egestas mollis ex quis semper. Nam vitae diam aliquet, elementum leo molestie, suscipit mauris. Suspendisse ex magna, fermentum eu sem non, convallis faucibus nisl. In id dignissim orci, non sodales ante. Nulla bibendum sem nec turpis porta pulvinar. Curabitur fringilla libero ut ex faucibus, non ultrices nisl volutpat. Quisque imperdiet condimentum diam eu scelerisque. Integer ullamcorper euismod accumsan. Curabitur efficitur, neque non blandit tempus, quam erat viverra risus, eu placerat risus elit eu neque. 

\pagebreak
\section*{Overall Exposure}";

        var parSumFormatted = string.Join("",
                parsedSummary.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();
        var testSumFormatted = string.Join("",
                testSummary.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();

        parSumFormatted.Should().Be(testSumFormatted);
    }
}