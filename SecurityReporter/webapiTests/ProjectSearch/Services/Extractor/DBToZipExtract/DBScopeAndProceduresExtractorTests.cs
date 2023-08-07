using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

[TestFixture]
public class DbScopeAndProceduresExtractorTests
{
    [Test]
    public void ExtractScopeAndProceduresTest()
    {
        // Arrange
        var extractor = new DbScopeAndProceduresExtractor();
        var scopeAndProcedures = new ScopeAndProcedures();

        // Scope Procedures - InScope
        var sp = new ScopeProcedure();
        sp.Component = "APK file";
        sp.Detail = "Android application";
        var sp1 = new ScopeProcedure();
        sp1.Component = "IPA file";
        sp1.Detail = "iOS application";

        scopeAndProcedures.InScope = new List<ScopeProcedure>
        {
            sp,
            sp1
        };

        // Worst Case Scenario - Scopes
        scopeAndProcedures.WorstCaseScenarios = new List<string>
        {
            "Information leakage of personal /patient data/customer data",
            "Modification or corruption of data",
            "Unauthorized read/write access to application/database",
            "Asset becomes partly or completely unavailable"
        };

        var outOfScopeSp = new ScopeProcedure
        {
            Component = "3rd party plugins",
            Detail = "Plugins not developed by Siemens"
        };
        var outOfScopeSp1 = new ScopeProcedure
        {
            Component = "Underlying operating systems",
            Detail = "Android and iOS"
        };

        scopeAndProcedures.OutOfScope = new List<ScopeProcedure>
        {
            outOfScopeSp,
            outOfScopeSp1
        };

        scopeAndProcedures.Environment = new List<string>
        {
            "Android APK file",
            "iOS IPA file",
            "application source code",
            "test user credentials."
        };

        // Act
        var result = DbScopeAndProceduresExtractor.ExtractScopeAndProcedures(scopeAndProcedures);
        var resultDecoded = Encoding.UTF8.GetString(result);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(resultDecoded);

        
        
    }
}