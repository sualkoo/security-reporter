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
        var extractor = new DbExecutiveSummaryExtractor();
        var inputStr = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pharetra pretium.";

        // Act
        var result = DbExecutiveSummaryExtractor.ExtractExecutiveSummary(inputStr);
        var resultDecoded = Encoding.UTF8.GetString(result);

        // Assert
        Assert.IsNotNull(result);
        StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tEXECUTIVE SUMMARY\r\n%----------------------------------------------------------------------------------------\r\n%-<ExecSum>->\r\n\r\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pharetra pretium.\r\n\r\n%-<ExecSum>\r\n\\pagebreak\r\n\\section*{Overall Exposure}", resultDecoded);
    }
}