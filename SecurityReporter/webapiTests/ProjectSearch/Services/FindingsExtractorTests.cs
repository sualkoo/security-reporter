using System.IO.Compression;
using FluentAssertions;
using NUnit.Framework;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

namespace webapiTests.ProjectSearch.Services
{
    [TestFixture()]
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
            testFinding.SubsectionReferences = @"\\href{https://doku-center.med.siemens.de/regelwerke/L4U-Intranet/GD/GD-41/GD-41-03-E.pdf}{Siemens Healthineers Guidance for Secure Software Architecture, Design andDevelopment: 8.4 Code-Signing}
                \\bibentry{CWE-296}";

            parsedFinding.Should().BeEquivalentTo(testFinding, options =>
            options
                .Excluding(str => str.Name == "SubsectionDetails")
                .Excluding(str => str.Name == "SubsectionImpact")
                .Excluding(str => str.Name == "SubsectionRepeatability")
                .Excluding(str => str.Name == "SubsectionCountermeasures")
                .Excluding(str => str.Name == "SubsectionReferences")
            );

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
            testFinding.SubsectionReferences = "";
            testFinding.SubsectionCountermeasures = "";
            parsedFinding.Should().BeEquivalentTo(testFinding, options =>
            options
                .Excluding(str => str.Name == "SubsectionDetails")
                .Excluding(str => str.Name == "SubsectionImpact")
                .Excluding(str => str.Name == "SubsectionRepeatability")
                .Excluding(str => str.Name == "SubsectionCountermeasures")
                .Excluding(str => str.Name == "SubsectionReferences")
            );
            //Assert.IsTrue(true);
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
            testFinding.SubsectionImpact = @"
Debug certificates do not meet security standards of the release certificates. 

%-<Impact>
%-------------------------------------------
%	Repeatability                          |
%-------------------------------------------
\pagebreak

The \texttt{dummyapplication.apk} is signed with a debug certificate (\texttt{CERT.RSA}), which can be found in the \texttt{META-INF} folder.
The certificate properties are shown in \cref{figure:DebugCert}.

\begin{figure}[H]
\centering
\includegraphics[width=0.7\textwidth,frame]{\CurrentFilePath/DebugCert.png}
\caption{Android debug certificate properties}
\label{figure:DebugCert}
\end{figure}
	

%-<Repeatability>
%-------------------------------------------
%	Countermeasures                        |
%-------------------------------------------


";
            testFinding.SubsectionCountermeasures = @"
Make sure that release version of the application is signed with the organization certificate of appropriate RSA (2048-bit)
and SHA-2 keysizes.

%-<Countermeasures>
%-------------------------------------------
%	References - pulls bib entries         |
%-------------------------------------------


This finding references the following information sources:

\begin{itemize}
	\item \href{https://doku-center.med.siemens.de/regelwerke/L4U-Intranet/GD/GD-41/GD-41-03-E.pdf}{Siemens Healthineers Guidance for Secure Software Architecture, Design and
	Development: 8.4 Code-Signing}
	\item \bibentry{CWE-296}
\end{itemize}

%-<References>

";

            parsedFinding.Should().BeEquivalentTo(testFinding, options =>
            options
                .Excluding(str => str.Name == "SubsectionDetails")
                .Excluding(str => str.Name == "SubsectionImpact")
                .Excluding(str => str.Name == "SubsectionRepeatability")
                .Excluding(str => str.Name == "SubsectionCountermeasures")
                .Excluding(str => str.Name == "SubsectionReferences")
            );

            string testImpact = String.Join("", testFinding.SubsectionImpact.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
            string testCountermeasures = String.Join("", testFinding.SubsectionCountermeasures.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();

            string parsedImpact = String.Join("", parsedFinding.SubsectionImpact.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
            string parsedCountermeasures = String.Join("", parsedFinding.SubsectionCountermeasures.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();

            testImpact.Should().Be(parsedImpact);
            testCountermeasures.Should().Be(parsedCountermeasures);

            entry = zipArchive.GetEntry("Findings/CommandsMissing/Only_Subsections_Left.tex");
            parsedFinding = fe.extractFinding(entry);
            testFinding = new Finding();
            testFinding.SubsectionDetails = @"
The \texttt{dummyapplication.apk} is signed with a debug certificate.

%-<Details>
%-------------------------------------------
%	Impact                                 |
%-------------------------------------------


";
            testFinding.SubsectionImpact = @"
Debug certificates do not meet security standards of the release certificates. 

%-<Impact>
%-------------------------------------------
%	Repeatability                          |
%-------------------------------------------
\pagebreak

";
            testFinding.SubsectionRepeatability = @"
The \texttt{dummyapplication.apk} is signed with a debug certificate (\texttt{CERT.RSA}), which can be found in the \texttt{META-INF} folder.
The certificate properties are shown in \cref{figure:DebugCert}.

\begin{figure}[H]
\centering
\includegraphics[width=0.7\textwidth,frame]{\CurrentFilePath/DebugCert.png}
\caption{Android debug certificate properties}
\label{figure:DebugCert}
\end{figure}
	

%-<Repeatability>
%-------------------------------------------
%	Countermeasures                        |
%-------------------------------------------


";
            testFinding.SubsectionCountermeasures = @"
Make sure that release version of the application is signed with the organization certificate of appropriate RSA (2048-bit)
and SHA-2 keysizes.

%-<Countermeasures>
%-------------------------------------------
%	References - pulls bib entries         |
%-------------------------------------------


";
            testFinding.SubsectionReferences = @"
This finding references the following information sources:

\begin{itemize}
	\item \href{https://doku-center.med.siemens.de/regelwerke/L4U-Intranet/GD/GD-41/GD-41-03-E.pdf}{Siemens Healthineers Guidance for Secure Software Architecture, Design and
	Development: 8.4 Code-Signing}
	\item \bibentry{CWE-296}
\end{itemize}

%-<References>

";

            parsedFinding.Should().BeEquivalentTo(testFinding, options =>
            options
                .Excluding(str => str.Name == "SubsectionDetails")
                .Excluding(str => str.Name == "SubsectionImpact")
                .Excluding(str => str.Name == "SubsectionRepeatability")
                .Excluding(str => str.Name == "SubsectionCountermeasures")
                .Excluding(str => str.Name == "SubsectionReferences")
            );

            string testDetails = String.Join("", testFinding.SubsectionDetails.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
            testImpact = String.Join("", testFinding.SubsectionImpact.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
            string testRepeatability = String.Join("", testFinding.SubsectionRepeatability.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
            testCountermeasures = String.Join("", testFinding.SubsectionCountermeasures.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
            string testReferences = String.Join("", testFinding.SubsectionReferences.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();

            string parsedDetails = String.Join("", parsedFinding.SubsectionDetails.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
            parsedImpact = String.Join("", parsedFinding.SubsectionImpact.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
            string parsedRepeatability = String.Join("", parsedFinding.SubsectionRepeatability.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
            parsedCountermeasures = String.Join("", parsedFinding.SubsectionCountermeasures.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
            string parsedReferences = String.Join("", parsedFinding.SubsectionReferences.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();

            testDetails.Should().Be(parsedDetails);
            testImpact.Should().Be(parsedImpact);
            testRepeatability.Should().Be(parsedRepeatability);
            testCountermeasures.Should().Be(parsedCountermeasures);
            testReferences.Should().Be(parsedReferences);

        }
    }
}