using NUnit.Framework;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract.Tests
{
    [TestFixture()]
    public class DBScopeAndProceduresExtractorTests
    {
        [Test()]
        public void extractScopeAndProceduresTest()
        {
            // Arrange
            DBScopeAndProceduresExtractor extractor = new DBScopeAndProceduresExtractor();
            ScopeAndProcedures scopeAndProcedures = new ScopeAndProcedures();

            // Scope Procedures - InScope
            var sp = new ScopeProcedure();
            sp.Component = "APK file";
            sp.Detail = "Android application";
            var sp1 = new ScopeProcedure();
            sp1.Component = "IPA file";
            sp1.Detail = "iOS application";

            scopeAndProcedures.InScope = new List<ScopeProcedure>()
            {
                sp,
                sp1
            };

            // Worst Case Scenario - Scopes
            scopeAndProcedures.WorstCaseScenarios = new List<string>()
            {
                "Information leakage of personal /patient data/customer data",
                "Modification or corruption of data",
                "Unauthorized read/write access to application/database",
                "Asset becomes partly or completely unavailable"
            };

            var outOfScopeSp = new ScopeProcedure();
            outOfScopeSp.Component = "3rd party plugins";
            outOfScopeSp.Detail = "Plugins not developed by Siemens";
            var outOfScopeSp1 = new ScopeProcedure();
            outOfScopeSp1.Component = "Underlying operating systems";
            outOfScopeSp1.Detail = "Android and iOS";

            scopeAndProcedures.OutOfScope = new List<ScopeProcedure>() 
            {
                outOfScopeSp,
                outOfScopeSp1
            };

            scopeAndProcedures.Environment = new List<string>() 
            {
                "Android APK file",
                "iOS IPA file",
                "application source code",
                "test user credentials."
            };

            // Act
            var result = extractor.extractScopeAndProcedures(scopeAndProcedures);
            var resultDecoded = Encoding.UTF8.GetString(result);

            Assert.IsNotNull(result);
            Assert.IsNotNull(resultDecoded);

            // Assert

            // In Scope
            StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tIN SCOPE\r\n%----------------------------------------------------------------------------------------", resultDecoded);
            StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tWORST CASE SCENARIOS\r\n%----------------------------------------------------------------------------------------", resultDecoded);
            StringAssert.Contains("% Not needed for Scope Document\r\n% Required for Report", resultDecoded);
            StringAssert.Contains("\\hline", resultDecoded);
            StringAssert.Contains("APK file & Android application \\\\", resultDecoded);
            StringAssert.Contains("IPA file & iOS application \\\\", resultDecoded);
            // Worst Case Scenarios
            StringAssert.Contains("\\newcommand{\\WorstCaseScenariosReport}{", resultDecoded);
            StringAssert.Contains("\\newcommand{\\WorstCaseScenariosReport}{\r\n\r\n\\begin{xltabular}{\\textwidth}{|l|X|c|c|c|c|c|c|}\r\n\t\\hline \r\n\t\\cellcolor{grey230}  \\textbf{Finding \\#} &\t \\cellcolor{grey230} \\textbf{Description} & \\cellcolor{grey230}\r\n\t\r\n\t\\textbf{WS1} & \\cellcolor{grey230} \\textbf{WS2} & \\cellcolor{grey230} \\textbf{WS3}\\\\\r\n    \\hline ", resultDecoded);
            // Worst Case Scenario items
            // Todo: StringAssert - for items
            StringAssert.Contains("\\caption{Findings case scenarios} \\label{table:FindingCaseScenarios}", resultDecoded);
            StringAssert.Contains("\\end{xltabular}\r\n}", resultDecoded);

            // Worst Case Scenarios Scope
            StringAssert.Contains("% Required for Scope & Report.", resultDecoded);
            StringAssert.Contains("\\newcommand{\\WorstCaseScenariosScope}{\r\n\t\\hline", resultDecoded);
            StringAssert.Contains("WS1\t&\tInformation leakage of personal /patient data/customer data\t\\\\", resultDecoded);
            StringAssert.Contains("WS2\t&\tModification or corruption of data\t\\\\", resultDecoded);
            StringAssert.Contains("WS3\t&\tUnauthorized read/write access to application/database\t\\\\", resultDecoded);
            StringAssert.Contains("WS4\t&\tAsset becomes partly or completely unavailable\t\\\\", resultDecoded);
            // Closing bracket & Comment
            StringAssert.Contains("}\r\n\r\n\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tOUT OF SCOPE\r\n%----------------------------------------------------------------------------------------", resultDecoded);
            StringAssert.Contains("\\newcommand{\\OutOfScope}{\r\n    \\hline", resultDecoded);
            StringAssert.Contains("\t3rd party plugins & Plugins not developed by Siemens \\\\", resultDecoded);
            StringAssert.Contains("\tUnderlying operating systems & Android and iOS \\\\", resultDecoded);
            StringAssert.Contains("}\r\n\r\n%----------------------------------------------------------------------------------------\r\n%\tENVIRONMENT\r\n%----------------------------------------------------------------------------------------", resultDecoded);
            StringAssert.Contains("\\newcommand{\\Environment}{", resultDecoded);
            StringAssert.Contains("    \\ReportAssessmentTeamLong TODOTODO.", resultDecoded);
            StringAssert.Contains("\t\\begin{itemize}", resultDecoded);
            StringAssert.Contains("\t\t\\item Android APK file,", resultDecoded);
            StringAssert.Contains("\t\t\\item iOS IPA file,", resultDecoded);
            StringAssert.Contains("\t\t\\item application source code,", resultDecoded);
            StringAssert.Contains("\t\t\\item test user credentials.", resultDecoded);
            StringAssert.Contains("\t\\end{itemize}\r\n}", resultDecoded);
            StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tTEST PROTOCOL: TARGET\r\n%----------------------------------------------------------------------------------------", resultDecoded);
            StringAssert.Contains("\\newcommand{\\TargetTestProtocol}{https://TODOTODO}", resultDecoded);
        }
    }
}