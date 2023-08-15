using System.Text;
using NUnit.Framework;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

[TestFixture]
public class DbExecutiveSummaryExtractorTests
{
    [Test]
    public void ExtractExecutiveSummaryTest()
    {
        // Arrange
        var inputStr = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pharetra pretium.";

        // Act
        var result = DbExecutiveSummaryExtractor.ExtractExecutiveSummary(inputStr);
        var resultDecoded = Encoding.UTF8.GetString(result);

        // Assert
        Assert.IsNotNull(result);
        StringAssert.Contains("%----------------------------------------------------------------------------------------\n%\tEXECUTIVE SUMMARY\n%----------------------------------------------------------------------------------------\n%-<ExecSum>->\n\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pharetra pretium.\n\n%-<ExecSum>\n\\pagebreak\n\\section*{Overall Exposure}", resultDecoded);
    }
}