using NUnit.Framework;
using System.Text;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract.Tests
{
    [TestFixture()]
    public class DBTestingMethodologyExtractorTests
    {
        [Test()]
        public void extractTestingMethodologyTest()
        {
            // Arrange
            DBTestingMethodologyExtractor extractor = new DBTestingMethodologyExtractor();
            TestingMethodology testingMethodology = new TestingMethodology();

            var t = new Tool();
            t.ToolName = "adb";
            t.Version = "1.0.41";
            t.TestType = "Android debugging";
            t.WorkType = "Bridge to Andorid device";

            var t1 = new Tool();
            t1.ToolName = "Android Studio";
            t1.Version = "2022.2.1";
            t1.TestType = "Android Development, Emulator";
            t1.WorkType = "Official integrated development environment for Google's Android operating system, with emulator capabilities.";

            testingMethodology.ToolsUsed = new List<Tool>() 
            {
                t,
                t1
            };

            testingMethodology.AttackVectors = new List<string>()
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
            var result = extractor.extractTestingMethodology(testingMethodology);
            var resultDecoded = Encoding.UTF8.GetString(result);

            // Assert
            Assert.IsNotNull(resultDecoded);

            StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tTOOLS USED\r\n%----------------------------------------------------------------------------------------\r\n% Not needed for Scope document\r\n% Required for Report document", resultDecoded);
            StringAssert.Contains("\\newcommand{\\ToolsUsed}{", resultDecoded);
            StringAssert.Contains("\t\\hline", resultDecoded);
            StringAssert.Contains("\tadb & 1.0.41 & Android debugging & Bridge to Andorid device \\\\", resultDecoded);
            StringAssert.Contains("\tAndroid Studio & 2022.2.1 & Android Development, Emulator & Official integrated development environment for Google's Android operating system, with emulator capabilities. \\\\", resultDecoded);
            StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tATTACK VECTORS AND PAYLOAD TYPES\r\n%----------------------------------------------------------------------------------------\r\n% Not needed for Scope document\r\n% Required for Report document", resultDecoded);
            StringAssert.Contains("\\newcommand{\\AttackVectors}{", resultDecoded);
            StringAssert.Contains("Tests on \\ReportProjectName included, but were not limited to:", resultDecoded);
            StringAssert.Contains("\begin{itemize}", resultDecoded);
            StringAssert.Contains("\t\t\\item static analysis,", resultDecoded);
            StringAssert.Contains("\t\t\\item file system analysis,", resultDecoded);
            StringAssert.Contains("\t\t\\item debugging,", resultDecoded);
            StringAssert.Contains("\t\t\\item workflow analysis,", resultDecoded);
            StringAssert.Contains("\t\t\\item client-side testing,", resultDecoded);
            StringAssert.Contains("\t\t\\item testing for weak cryptography,", resultDecoded);
            StringAssert.Contains("\t\t\\item testing error handling.", resultDecoded);
            StringAssert.Contains("\t\\end{itemize}\r\n\r\n}", resultDecoded);
        }
    }
}