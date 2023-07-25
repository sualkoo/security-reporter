using NUnit.Framework;
using webapi.ProjectSearch.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Tests
{
    [TestFixture()]
    public class DocumentInformationExtractorTests
    {
        ZipArchive testsZip = null;
        Stream zipStream = null;
        List<object> correctValues = new List<object>
        {
            "Dummy Project 1",
            "Mobile Application",
            "Lukas Nad",

        };

        public DocumentInformationExtractorTests() 
        { 
            using(ZipArchive archive = ZipFile.OpenRead("../ParserTestResources/parserUnitTestsZip.zip"))
            {
                if(archive != null)
                {
                    ZipArchiveEntry entry = archive.GetEntry("Document_Information.tex");
                    if(entry != null)
                    {
                        bool[] correct = { true, true, true, true, true, true, true, true, true };
                        bool[] mae1 = { false, false, true, false, true, true, true};
                        int mae1RDHLen = 2;
                        ExtractDocumentInformationTest(entry, mae1, mae1RDHLen);
                    }
                }
            }

        }
        [Test()]
        public void ExtractDocumentInformationTest(ZipArchiveEntry entry, bool[] conditions, int reportDocumentHistoryLength)
        {
            try
            {
                DocumentInformationExtractor die = new DocumentInformationExtractor(entry);
                DocumentInformation testDI = die.ExtractDocumentInformation();
                for(int i = 0; i < conditions.Length; i++)
                {
                    switch(i)
                    {
                        case 0:
                            if ((testDI.ProjectReportName != null) == conditions[i])
                    }
                }
            } catch(Exception ex)
            {
                Assert.Fail();
            }
        }
    }
}