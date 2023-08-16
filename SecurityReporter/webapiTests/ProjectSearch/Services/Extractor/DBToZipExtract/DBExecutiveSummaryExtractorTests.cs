using System.Text;
using NUnit.Framework;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;
using StringAssert = Microsoft.VisualStudio.TestTools.UnitTesting.StringAssert;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

[TestFixture]
public class DbExecutiveSummaryExtractorTests
{
    [Test]
    public void ExtractExecutiveSummaryTest()
    {
        // Arrange
        var inputStr = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pharetra pretium.";

        string expectedStr = @"%----------------------------------------------------------------------------------------
%	EXECUTIVE SUMMARY
%----------------------------------------------------------------------------------------
%-<ExecSum>->

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pharetra pretium.

%-<ExecSum>
\pagebreak
\section*{Overall Exposure}";
        
        // Act
        var result = DbExecutiveSummaryExtractor.ExtractExecutiveSummary(inputStr);
        var resultDecoded = Encoding.UTF8.GetString(result);

        // Assert
        Assert.IsNotNull(result);
        StringAssert.Contains(expectedStr, resultDecoded);
    }
}