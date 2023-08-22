using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using webapi.ProjectSearch.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

[TestFixture]
public class DbTestingMethodologyExtractorTests
{
	[Test]
	public void ExtractTestingMethodologyTest()
	{
		// Arrange
		var testingMethodology = new TestingMethodology();
		testingMethodology.ToolsUsed = @"
	\hline 
	adb & 1.0.41 & Android debugging & Bridge to Android device \\
	\hline
	Android Studio & 2022.2.1 & Android Development, Emulator & Official integrated development environment for Google's Android operating system, with emulator capabilities. \\
	\hline";


        testingMethodology.AttackVectors = @"

	Tests on \ReportProjectName included, but were not limited to:

	\begin{itemize}
		\item static analysis,
		\item file system analysis,
		\item debugging,
		\item workflow analysis,
		\item client-side testing,
		\item testing for weak cryptography,
		\item testing error handling.
	\end{itemize}
";

        

        var expectedStr = @"%----------------------------------------------------------------------------------------
%	TOOLS USED
%----------------------------------------------------------------------------------------
% Not needed for Scope document
% Required for Report document
\newcommand{\ToolsUsed}{
	\hline 
	adb & 1.0.41 & Android debugging & Bridge to Android device \\
	\hline
	Android Studio & 2022.2.1 & Android Development, Emulator & Official integrated development environment for Google's Android operating system, with emulator capabilities. \\
	\hline
} 


%----------------------------------------------------------------------------------------
%	ATTACK VECTORS AND PAYLOAD TYPES
%----------------------------------------------------------------------------------------
% Not needed for Scope document
% Required for Report document
\newcommand{\AttackVectors}{

	Tests on \ReportProjectName included, but were not limited to:

	\begin{itemize}
		\item static analysis,
		\item file system analysis,
		\item debugging,
		\item workflow analysis,
		\item client-side testing,
		\item testing for weak cryptography,
		\item testing error handling.
	\end{itemize}

}";

        // Act
        var result = DbTestingMethodologyExtractor.ExtractTestingMethodology(testingMethodology);
        var resultDecoded = Encoding.UTF8.GetString(result);

        // Assert
        Assert.IsNotNull(resultDecoded);
        var resultDecodedFormatted = string.Join("", 
			resultDecoded.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();
        var testFormatted = string.Join("",
                expectedStr.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();

		resultDecodedFormatted.Should().Be(testFormatted);
    }
}