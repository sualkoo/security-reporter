using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

[TestFixture]
public class DbTestingMethodologyExtractorTests
{
    [Test]
    public void ExtractTestingMethodologyTest()
    {
        // Arrange
        var testingMethodology = new TestingMethodology();

        var t = new Tool();
        t.ToolName = "adb";
        t.Version = "1.0.41";
        t.TestType = "Android debugging";
        t.WorkType = "Bridge to Andorid device";

        var t1 = new Tool();
        t1.ToolName = "Android Studio";
        t1.Version = "2022.2.1";
        t1.TestType = "Android Development, Emulator";
        t1.WorkType =
            "Official integrated development environment for Google's Android operating system, with emulator capabilities.";

        testingMethodology.ToolsUsed = new List<Tool>
        {
            t,
            t1
        };

        testingMethodology.AttackVectors = new List<string>
        {
            "static analysis",
            "file system analysis",
            "debugging",
            "workflow analysis",
            "client-side testing",
            "testing for weak cryptography",
            "testing error handling."
        };

        // Act
        var result = DbTestingMethodologyExtractor.ExtractTestingMethodology(testingMethodology);
        var resultDecoded = Encoding.UTF8.GetString(result);

        // Assert
        Assert.IsNotNull(resultDecoded);
        StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tTOOLS USED\r\n%----------------------------------------------------------------------------------------\r\n% Not needed for Scope document\r\n% Required for Report document\r\n\\newcommand{\\ToolsUsed}{\r\n\t\\hline \r\n\tadb & 1.0.41 & Android debugging & Bridge to Andorid device \\\\\n\t\\hline\n\tAndroid Studio & 2022.2.1 & Android Development, Emulator & Official integrated development environment for Google's Android operating system, with emulator capabilities. \\\\\n\t\\hline\r\n} \r\n\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tATTACK VECTORS AND PAYLOAD TYPES\r\n%----------------------------------------------------------------------------------------\r\n% Not needed for Scope document\r\n% Required for Report document\r\n\\newcommand{\\AttackVectors}{\r\n\r\n\tTests on \\ReportProjectName included, but were not limited to:\r\n\r\n\t\\begin{itemize}\r\n\t\t\\item static analysis,\n\t\t\\item file system analysis,\n\t\t\\item debugging,\n\t\t\\item workflow analysis,\n\t\t\\item client-side testing,\n\t\t\\item testing for weak cryptography,\n\t\t\\item testing error handling.\r\n\t\\end{itemize}\r\n\r\n}", resultDecoded);
    }
}