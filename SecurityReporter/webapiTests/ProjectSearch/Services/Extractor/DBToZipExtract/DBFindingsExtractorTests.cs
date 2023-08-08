using System.IO;
using NUnit.Framework;
using webapi.ProjectSearch.Services;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

[TestFixture]
[TestFixture]
public class DbFindingsExtractorTests
{
    [Test]
    public void ExtractFindingTest()
    {
        //zipArchive = ZipFile.OpenRead("../../../ProjectSearch/ParserTestResources/parserUnitTestsZip.zip");
        //Assert.IsNotNull(zipArchive);
        //ZipArchiveEntry entry = zipArchive.GetEntry("Findings/FullInformation/Finding.tex");
        //FindingsExtractor fe = new FindingsExtractor();
        //Assert.IsNotNull(fe);
        var pdp = new ProjectDataParser();
        using (var filestream = new FileStream("C:\\Users\\user\\Downloads\\dobre.zip", FileMode.Open, FileAccess.Read))
        {
            var data = pdp.Extract(filestream);
            var parsedFinding = data.Findings[0];
            var result = DbFindingsExtractor.ExtractFinding(parsedFinding);
            var createdDirectory = Directory.CreateDirectory("C:\\Users\\user\\Downloads\\resultTest");

            File.WriteAllBytes(createdDirectory + "//main.tex", result.Item1.ToArray());
            foreach (var item in result.Item2)
            {
                var filePath = Path.Combine("C:\\Users\\user\\Downloads\\resultTest\\" + item.FileName);
                // item.image.Save(filePath, ImageFormat.Png);
            }
        }
    }
}