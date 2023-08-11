using System.IO.Compression;
using FluentAssertions;
using NUnit.Framework;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

namespace webapiTests.ProjectSearch.Services
{
    [TestFixture()]
    [Ignore("")]
    public class FindingsExtractorTests
    {
        private ZipArchive zipArchive;
        [SetUp()]
        public void SetUp()
        {
            zipArchive = ZipFile.OpenRead("../../../ProjectSearch/ParserTestResources/parserUnitTestsZip.zip");
            Assert.IsNotNull(zipArchive);
        }

        [TearDown()]
        public void TearDown()
        {
            zipArchive.Dispose();
        }

        [Test()]
        public void FullInformation()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("Findings/FullInformation/Finding.tex");
            FindingsExtractor fe = new FindingsExtractor();
            Assert.IsNotNull(fe);
            Finding parsedFinding = fe.extractFinding(entry);
            Finding testFinding = new Finding();
            testFinding.FindingAuthor = "Lukas Nad";
            testFinding.FindingName = "DummyApplication Signed with a Debug Certificate";
            testFinding.Location = new List<string> { "dummyapplication.apk" };
            testFinding.Component = "META-INF/CERT.RSA";
            testFinding.FoundWith = "KeyStore Explorer";
            testFinding.TestMethod = "Manual Analysis";
            testFinding.CVSS = "N/A";
            testFinding.CVSSVector = "N/A";
            testFinding.CWE = 296;
            testFinding.Criticality = webapi.Enums.Criticality.Info;
            testFinding.Exploitability = webapi.Enums.Exploitability.Hard;
            testFinding.Category = webapi.Enums.Category.SecurityConfiguration;
            testFinding.Detectability = webapi.Enums.Detectability.Difficult;
            testFinding.SubsectionDetails = "The \\texttt{dummyapplication.apk} is signed with a debug certificate.";
            testFinding.SubsectionImpact = "Debug certificates do not meet security standards of the release certificates.";
            testFinding.SubsectionRepeatability = "The \\texttt{dummyapplication.apk} is signed with a debug certificate (\\texttt{CERT.RSA}), " +
                "which can be found in the \\texttt{META-INF} folder.The certificate properties are shown in \\cref{figure:DebugCert}.";
            testFinding.SubsectionCountermeasures = "Make sure that release version of the application is signed with the organization certificate of appropriate RSA (2048-bit)and SHA-2 keysizes.";
            testFinding.SubsectionReferences = @"
This finding references the following information sources:

\begin{itemize}
	\item \href{https://doku-center.med.siemens.de/regelwerke/L4U-Intranet/GD/GD-41/GD-41-03-E.pdf}{Siemens Healthineers Guidance for Secure Software Architecture, Design and
	Development: 8.4 Code-Signing}
	\item \bibentry{CWE-296}
\end{itemize}

%-<References>";

            parsedFinding.Should().BeEquivalentTo(testFinding, options =>
            options
                .Excluding(str => str.Name == "SubsectionDetails")
                .Excluding(str => str.Name == "SubsectionImpact")
                .Excluding(str => str.Name == "SubsectionRepeatability")
                .Excluding(str => str.Name == "SubsectionCountermeasures")
                .Excluding(str => str.Name == "SubsectionReferences")
            );

        }

        [Test()]
        public void Empty()
        {
            ZipArchiveEntry entry = zipArchive.GetEntry("Findings/Empty/Finding.tex");
            FindingsExtractor fe = new FindingsExtractor();
            Assert.IsNotNull(fe);
            Finding parsedFinding = fe.extractFinding(entry);
            Finding testFinding = new Finding();


            parsedFinding.Should().BeEquivalentTo(testFinding);
        }

         [Test()]
         public void CommandsEmpty()
         {
             ZipArchiveEntry entry = zipArchive.GetEntry("Findings/CommandsEmpty/Missing_renewcommand.tex");
             FindingsExtractor fe = new FindingsExtractor();
             Assert.IsNotNull(fe);
             Finding parsedFinding = fe.extractFinding(entry);
             Finding testFinding = new Finding();

             testFinding.SubsectionDetails = "The \\texttt{dummyapplication.apk} is signed with a debug certificate.";
             testFinding.SubsectionImpact = "Debug certificates do not meet security standards of the release certificates.";
             testFinding.SubsectionRepeatability = "The \\texttt{dummyapplication.apk} is signed with a debug certificate (\\texttt{CERT.RSA}), " +
                 "which can be found in the \\texttt{META-INF} folder.The certificate properties are shown in \\cref{figure:DebugCert}.";
             testFinding.SubsectionCountermeasures = "Make sure that release version of the application is signed with the organization certificate of appropriate RSA (2048-bit)and SHA-2 keysizes.";
             /*testFinding.SubsectionReferences = new List<string>
             {

                 "\\href{https://doku-center.med.siemens.de/regelwerke/L4U-Intranet/GD/GD-41/GD-41-03-E.pdf}{Siemens Healthineers Guidance for Secure Software Architecture, Design andDevelopment: 8.4 Code-Signing}",
                 "\\bibentry{CWE-296}"
             };*/

             parsedFinding.Should().BeEquivalentTo(testFinding);

             entry = zipArchive.GetEntry("Findings/CommandsEmpty/FA_Lo_CWE_Cri_Cat_Det_Counter_Ref.tex");
             fe = new FindingsExtractor();
             Assert.IsNotNull(fe);
             parsedFinding = fe.extractFinding(entry);
             testFinding = new Finding();
             testFinding.FindingName = "DummyApplication Signed with a Debug Certificate";
             testFinding.Component = "META-INF/CERT.RSA";
             testFinding.FoundWith = "KeyStore Explorer";
             testFinding.TestMethod = "Manual Analysis";
             testFinding.CVSS = "N/A";
             testFinding.CVSSVector = "N/A";
             testFinding.Criticality = webapi.Enums.Criticality.Info;
             testFinding.Exploitability = webapi.Enums.Exploitability.Hard;
             testFinding.SubsectionDetails = "The \\texttt{dummyapplication.apk} is signed with a debug certificate.";
             testFinding.SubsectionImpact = "Debug certificates do not meet security standards of the release certificates.";
             testFinding.SubsectionRepeatability = "";
             //testFinding.SubsectionReferences = new List<string>();
             testFinding.SubsectionCountermeasures = "";
             parsedFinding.Should().BeEquivalentTo(testFinding);
         }

         [Test()]
         public void CommandsMissing()
         {
             ZipArchiveEntry entry = zipArchive.GetEntry("Findings/CommandsMissing/All_Commands_Missing.tex");
             FindingsExtractor fe = new FindingsExtractor();
             Assert.IsNotNull(fe);
             Finding parsedFinding = fe.extractFinding(entry);
             Finding testFinding = new Finding();
             parsedFinding.Should().BeEquivalentTo(testFinding);

             entry = zipArchive.GetEntry("Findings/CommandsMissing/FA_L_CWE_Ex_De_Rep_Ref.tex");
             parsedFinding = fe.extractFinding(entry);
             testFinding = new Finding();
             testFinding.FindingName = "DummyApplication Signed with a Debug Certificate";
             testFinding.Component = "META-INF/CERT.RSA";
             testFinding.FoundWith = "KeyStore Explorer";
             testFinding.TestMethod = "Manual Analysis";
             testFinding.CVSS = "N/A";
             testFinding.CVSSVector = "N/A";
             testFinding.Criticality = webapi.Enums.Criticality.Info;
             testFinding.Category = webapi.Enums.Category.SecurityConfiguration;
             testFinding.Detectability = webapi.Enums.Detectability.Difficult;
             testFinding.SubsectionImpact = "Debug certificates do not meet security standards of the release certificates.";
             
             testFinding.SubsectionCountermeasures = "Make sure that release version of the application is signed with the organization certificate of appropriate RSA (2048-bit)and SHA-2 keysizes.";


             parsedFinding.Should().BeEquivalentTo(testFinding);



             entry = zipArchive.GetEntry("Findings/CommandsMissing/Only_Subsections_Left.tex");
             parsedFinding = fe.extractFinding(entry);
             testFinding = new Finding();
             testFinding.SubsectionDetails = "The \\texttt{dummyapplication.apk} is signed with a debug certificate.";
             testFinding.SubsectionImpact = "Debug certificates do not meet security standards of the release certificates.";
             testFinding.SubsectionRepeatability = "The \\texttt{dummyapplication.apk} is signed with a debug certificate (\\texttt{CERT.RSA}), " +
                 "which can be found in the \\texttt{META-INF} folder.The certificate properties are shown in \\cref{figure:DebugCert}.";
             testFinding.SubsectionCountermeasures = "Make sure that release version of the application is signed with the organization certificate of appropriate RSA (2048-bit)and SHA-2 keysizes.";
             /*testFinding.SubsectionReferences = new List<string>
             {

                 "\\href{https://doku-center.med.siemens.de/regelwerke/L4U-Intranet/GD/GD-41/GD-41-03-E.pdf}{Siemens Healthineers Guidance for Secure Software Architecture, Design andDevelopment: 8.4 Code-Signing}",
                 "\\bibentry{CWE-296}"
             };*/

             parsedFinding.Should().BeEquivalentTo(testFinding);
         }
    }
}