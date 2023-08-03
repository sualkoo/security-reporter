using NUnit.Framework;
using System.IO.Compression;
using webapi.Models.ProjectReport;
using FluentAssertions;

namespace webapi.ProjectSearch.Services.Tests
{
    [TestFixture()]
    public class ScopeAndProceduresExtractorTests
    {
        ZipArchive zipArchive;
        [SetUp()]
        public void SetUp()
        {
            zipArchive = ZipFile.OpenRead("../../../ProjectSearch/ParserTestResources/parserUnitTestsZip.zip");
            Assert.IsNotNull(zipArchive);
        }

        [TearDown()]
        public void TearDown() {
            zipArchive.Dispose();
        }

        [Test()]
        public void Empty()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("ScopeAndProcedures/Empty/Scope_And_Procedures.tex");
            ScopeAndProceduresExtractor sape = new ScopeAndProceduresExtractor(entry);
            Assert.IsNotNull(sape);
        }

        [Test()]
        public void FullInformation()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("ScopeAndProcedures/FullInformation/Scope_And_Procedures.tex");
            ScopeAndProceduresExtractor sape = new ScopeAndProceduresExtractor(entry);
            Assert.IsNotNull(sape);
            ScopeAndProcedures parsedScope = sape.ExtractScopeAndProcedures();
            ScopeAndProcedures testScope = new ScopeAndProcedures();
            testScope.InScope = new List<ScopeProcedure>();
            ScopeProcedure inScope1 = new ScopeProcedure();
            inScope1.Component = "APK file";
            inScope1.Detail = "Android application";
            ScopeProcedure inScope2 = new ScopeProcedure();
            inScope2.Component = "IPA file";
            inScope2.Detail = "iOS application";
            ScopeProcedure inScope3 = new ScopeProcedure();
            inScope3.Component = "Source code";
            inScope3.Detail = "Static analysis";
            testScope.InScope.Add(inScope1);
            testScope.InScope.Add(inScope2);
            testScope.InScope.Add(inScope3);

            testScope.OutOfScope = new List<ScopeProcedure>();
            ScopeProcedure outOfScope1 = new ScopeProcedure();
            outOfScope1.Component = "3rd party plugins";
            outOfScope1.Detail = "Plugins not developed by Siemens";
            ScopeProcedure outOfScope2 = new ScopeProcedure();
            outOfScope2.Component = "Underlying operating systems";
            outOfScope2.Detail = "Android and iOS";
            ScopeProcedure outOfScope3 = new ScopeProcedure();
            outOfScope3.Component = "REST APIs";
            outOfScope3.Detail = "Was already tested";
            testScope.OutOfScope.Add(outOfScope1);
            testScope.OutOfScope.Add(outOfScope2);
            testScope.OutOfScope.Add(outOfScope3);

            testScope.WorstCaseScenarios = new List<string>
            {
                "Information leakage of personal /patient data/customer data",
                "Modification or corruption of data",
                "Unauthorized read/write access to application/database",
                "Asset becomes partly or completely unavailable"
            };

            testScope.Environment = new List<string>
            {
                "Android APK file",
                "iOS IPA file",
                "application source code",
                "test user credentials."
            };

            parsedScope.WorstCaseScenariosReport = null;
            testScope.WorstCaseScenariosReport = null;

            parsedScope.Should().BeEquivalentTo(testScope);
        }
    }
}