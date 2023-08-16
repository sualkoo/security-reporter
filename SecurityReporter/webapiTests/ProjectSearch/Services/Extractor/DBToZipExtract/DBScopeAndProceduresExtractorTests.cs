using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

namespace webapiTests.ProjectSearch.Services.Extractor.DBToZipExtract;

[TestFixture]
public class DbScopeAndProceduresExtractorTests
{
    [Test]
    public void ExtractScopeAndProceduresTest()
    {
        // Arrange
        var scopeAndProcedures = new ScopeAndProcedures();

        // Scope Procedures - InScope
        var sp = new ScopeProcedure();
        sp.Component = "APK file";
        sp.Detail = "Android application";
        var sp1 = new ScopeProcedure();
        sp1.Component = "IPA file";
        sp1.Detail = "iOS application";

        scopeAndProcedures.InScope = new List<ScopeProcedure>
        {
            sp,
            sp1
        };

        scopeAndProcedures.WorstCaseScenariosReport = @"\newcommand{\WorstCaseScenariosReport}{

\begin{xltabular}{\textwidth}{|l|X|c|c|c|c|c|c|c|}
	\hline 
	\cellcolor{grey230}  \textbf{Finding \#} &	 \cellcolor{grey230} \textbf{Description} & \cellcolor{grey230}
	
	\textbf{WS1} & \cellcolor{grey230} \textbf{WS2} & \cellcolor{grey230} \textbf{WS3} & \cellcolor{grey230} \textbf{WS4}\\
    \hline 
	1		&	ePHI is stored on device without encryption   & \Huge $\cdotp$ & \Huge $\cdotp$ & \Huge \phantom{} & \Huge \phantom{} \\
	\hline
	2		&	Sensitive Information Disclosure via Logging   & \Huge $\cdotp$ & \Huge $\cdotp$ & \Huge $\cdotp$  & \Huge \phantom{} \\
	\hline
	3		&	Weak Application Signature   & \Huge \phantom{} & \Huge \phantom{} & \Huge $\cdotp$  & \Huge $\cdotp$ \\
	\hline
	4		&	Heap Inspection of Sensitive Memory   & \Huge $\cdotp$ & \Huge $\cdotp$ & \Huge $\cdotp$  & \Huge \phantom{} \\
	\hline
	5		&	Outdated Components   & \Huge \phantom{} & \Huge \phantom{} & \Huge \phantom{}  & \Huge \phantom{} \\
	\hline
	6		&	DummyApplication Signed with a Debug Certificate   & \Huge \phantom{} & \Huge \phantom{} & \Huge \phantom{} & \Huge \phantom{} \\
	\hline
	7		&	Missing Enforced Updating   & \Huge \phantom{} & \Huge \phantom{} & \Huge \phantom{} & \Huge \phantom{} \\
	\hline
	
	\caption{Findings case scenarios} \label{table:FindingCaseScenarios}
\end{xltabular}
}";

        // Worst Case Scenario - Scopes
        scopeAndProcedures.WorstCaseScenarios = new List<string>
        {
            "Information leakage of personal /patient data/customer data",
            "Modification or corruption of data",
            "Unauthorized read/write access to application/database",
            "Asset becomes partly or completely unavailable"
        };

        var outOfScopeSp = new ScopeProcedure
        {
            Component = "3rd party plugins",
            Detail = "Plugins not developed by Siemens"
        };
        var outOfScopeSp1 = new ScopeProcedure
        {
            Component = "Underlying operating systems",
            Detail = "Android and iOS"
        };

        scopeAndProcedures.OutOfScope = new List<ScopeProcedure>
        {
            outOfScopeSp,
            outOfScopeSp1
        };

        scopeAndProcedures.Environment = new List<string>
        {
            "Android APK file",
            "iOS IPA file",
            "application source code",
            "test user credentials."
        };

        string expectedStr = @"%----------------------------------------------------------------------------------------
%	IN SCOPE
%----------------------------------------------------------------------------------------
\newcommand{\InScope}{

	\hline
	APK file & Android application \\
	IPA file & iOS application \\						
}


%----------------------------------------------------------------------------------------
%	WORST CASE SCENARIOS
%----------------------------------------------------------------------------------------

% Not needed for Scope Document
% Required for Report


\newcommand{\WorstCaseScenariosReport}{

\begin{xltabular}{\textwidth}{|l|X|c|c|c|c|c|c|c|}
	\hline 
	\cellcolor{grey230}  \textbf{Finding \#} &	 \cellcolor{grey230} \textbf{Description} & \cellcolor{grey230}
	
	\textbf{WS1} & \cellcolor{grey230} \textbf{WS2} & \cellcolor{grey230} \textbf{WS3} & \cellcolor{grey230} \textbf{WS4}\\
    \hline 
	1		&	ePHI is stored on device without encryption   & \Huge $\cdotp$ & \Huge $\cdotp$ & \Huge \phantom{} & \Huge \phantom{} \\
	\hline
	2		&	Sensitive Information Disclosure via Logging   & \Huge $\cdotp$ & \Huge $\cdotp$ & \Huge $\cdotp$  & \Huge \phantom{} \\
	\hline
	3		&	Weak Application Signature   & \Huge \phantom{} & \Huge \phantom{} & \Huge $\cdotp$  & \Huge $\cdotp$ \\
	\hline
	4		&	Heap Inspection of Sensitive Memory   & \Huge $\cdotp$ & \Huge $\cdotp$ & \Huge $\cdotp$  & \Huge \phantom{} \\
	\hline
	5		&	Outdated Components   & \Huge \phantom{} & \Huge \phantom{} & \Huge \phantom{}  & \Huge \phantom{} \\
	\hline
	6		&	DummyApplication Signed with a Debug Certificate   & \Huge \phantom{} & \Huge \phantom{} & \Huge \phantom{} & \Huge \phantom{} \\
	\hline
	7		&	Missing Enforced Updating   & \Huge \phantom{} & \Huge \phantom{} & \Huge \phantom{} & \Huge \phantom{} \\
	\hline
	
	\caption{Findings case scenarios} \label{table:FindingCaseScenarios}
\end{xltabular}
}


% Required for Scope & Report.
\newcommand{\WorstCaseScenariosScope}{
	\hline
	WS1	&	Information leakage of personal /patient data/customer data	\\
	WS2	&	Modification or corruption of data	\\
	WS3	&	Unauthorized read/write access to application/database	\\
	WS4	&	Asset becomes partly or completely unavailable	\\
}



%----------------------------------------------------------------------------------------
%	OUT OF SCOPE
%----------------------------------------------------------------------------------------
\newcommand{\OutOfScope}{
    \hline
	3rd party plugins & Plugins not developed by Siemens \\
	Underlying operating systems & Android and iOS \\
}

%----------------------------------------------------------------------------------------
%	ENVIRONMENT
%----------------------------------------------------------------------------------------
\newcommand{\Environment}{
    
    \ReportAssessmentTeamLong TODOTODO.
	\begin{itemize}
		\item Android APK file,
		\item iOS IPA file,
		\item application source code,
		\item test user credentials.
	\end{itemize}
}

%----------------------------------------------------------------------------------------
%	TEST PROTOCOL: TARGET
%----------------------------------------------------------------------------------------
\newcommand{\TargetTestProtocol}{https://TODOTODO}
";

        // Act
        var result = DbScopeAndProceduresExtractor.ExtractScopeAndProcedures(scopeAndProcedures);
        var resultDecoded = Encoding.UTF8.GetString(result);
        
        // Assert
        Assert.IsNotNull(result);
        StringAssert.Contains(StringNormalizer.Normalize(expectedStr), StringNormalizer.Normalize(resultDecoded));
    }
}