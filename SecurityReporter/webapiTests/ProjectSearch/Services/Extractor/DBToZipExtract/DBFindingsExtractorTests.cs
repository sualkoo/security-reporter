using FluentAssertions;
using NUnit.Framework;
using System.IO.Compression;
using System.Text;
using webapi.ProjectSearch.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

[TestFixture]
public class DbFindingsExtractorTests
{
    [Test]
    public void ExtractFindingTest()
    {
        string expectedStr = File.ReadAllText("../../../ProjectSearch/ParserTestResources/Findings/CommandsEmpty/FA_Lo_CWE_Cri_Cat_Det_Counter_Ref.tex");
        ZipArchive zipArchive = ZipFile.OpenRead("../../../ProjectSearch/ParserTestResources/parserUnitTestsZip.zip");
        FindingsExtractor fe = new FindingsExtractor();
        Assert.IsNotNull(zipArchive);
        ZipArchiveEntry entry = zipArchive.GetEntry("Findings/CommandsEmpty/FA_Lo_CWE_Cri_Cat_Det_Counter_Ref.tex");
        Assert.IsNotNull(entry);
        Finding parsedFinding = fe.ExtractFinding(entry);
        var result = DbFindingsExtractor.ExtractFinding(parsedFinding);
        string resultDecoded = Encoding.UTF8.GetString(result.Item1.ToArray());
        Assert.IsNotNull(parsedFinding);


        var resultDecodedFormatted = string.Join("",
    resultDecoded.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
    .ToLowerInvariant();
        var testFormatted = string.Join("",
                expectedStr.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();

        resultDecodedFormatted.Should().Be(testFormatted);

        /*using( var fileStream = new FileStream(entry))
        {

        }
        var pdp = new ProjectDataParser();
        var data = = pdp.Extract(new FileStream(entry, FileMode.Open, FileAccess.Read))*/
        //using (var filestream = new FileStream("C:\\Users\\user\\Downloads\\dobre.zip", FileMode.Open, FileAccess.Read))
        //{
        //    var data = pdp.Extract(filestream);
        //    var parsedFinding = data.Findings[0];
        //    var result = DbFindingsExtractor.ExtractFinding(parsedFinding);
        //    var createdDirectory = Directory.CreateDirectory("C:\\Users\\user\\Downloads\\resultTest");

        //    File.WriteAllBytes(createdDirectory + "//main.tex", result.Item1.ToArray());
        //    foreach (var item in result.Item2)
        //    {
        //        var filePath = Path.Combine("C:\\Users\\user\\Downloads\\resultTest\\" + item.FileName);
        //        // item.image.Save(filePath, ImageFormat.Png);
        //    }
        //}
        //Assert.IsTrue(true);
    }
}