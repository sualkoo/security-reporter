using NUnit.Framework;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract.Tests
{
    [TestFixture()]
    public class DBExecutiveSummaryExtractorTests
    {
        [Test()]
        public void extractExecutiveSummaryTest()
        {
            // Arrange
            DBExecutiveSummaryExtractor extractor = new DBExecutiveSummaryExtractor();
            string inputStr = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultricies pharetra pretium.";

            // Act
            var result = extractor.extractExecutiveSummary(inputStr);
            var resultDecoded = Encoding.UTF8.GetString(result);

            // Assert
            Assert.IsNotNull(result);
            StringAssert.Contains(inputStr, resultDecoded);
            StringAssert.Contains("%----------------------------------------------------------------------------------------\r\n%\tEXECUTIVE SUMMARY\r\n%----------------------------------------------------------------------------------------",
                resultDecoded);
            StringAssert.Contains("%-<ExecSum>->", resultDecoded);
            StringAssert.Contains("%-<ExecSum>", resultDecoded);
            StringAssert.Contains("\\pagebreak", resultDecoded);
            StringAssert.Contains("\\section*{Overall Exposure}", resultDecoded);
        }
    }
}