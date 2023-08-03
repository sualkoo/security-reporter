using NUnit.Framework;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;
using webapi.ProjectSearch.Models;
using Microsoft.Azure.Cosmos;
using System.Drawing.Imaging;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract.Tests
{
    [TestFixture()]
    public class DBFindingsExtractorTests
    {
        private ZipArchive zipArchive;
        [Test()]
        public void extractFindingTest()
        {
            //zipArchive = ZipFile.OpenRead("../../../ProjectSearch/ParserTestResources/parserUnitTestsZip.zip");
            //Assert.IsNotNull(zipArchive);
            //ZipArchiveEntry entry = zipArchive.GetEntry("Findings/FullInformation/Finding.tex");
            //FindingsExtractor fe = new FindingsExtractor();
            //Assert.IsNotNull(fe);
            ProjectDataParser pdp = new ProjectDataParser();
            using (FileStream filestream = new FileStream("C:\\Users\\user\\Downloads\\dobre.zip", FileMode.Open, FileAccess.Read))
            {
                ProjectReportData data = pdp.Extract(filestream);
                Finding parsedFinding = data.Findings[0];
                DBFindingsExtractor dfe = new DBFindingsExtractor(parsedFinding);
                var result = dfe.extractFinding();
                DirectoryInfo createdDirectory = Directory.CreateDirectory("C:\\Users\\user\\Downloads\\resultTest");

                File.WriteAllBytes(createdDirectory + "//main.tex", result.Item1.ToArray());
                foreach(var item in result.Item2)
                {
                    string filePath = Path.Combine("C:\\Users\\user\\Downloads\\resultTest\\" + item.FileName);
                    item.image.Save(filePath, ImageFormat.Png);
                }
            }

        }
    }
}