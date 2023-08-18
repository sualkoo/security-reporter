using System.Text;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

public static class DbScopeAndProceduresExtractor
{
    public static byte[] ExtractScopeAndProcedures(ScopeAndProcedures scopeAndProcedures)
    {
        var inScope = new List<string>();
        if (scopeAndProcedures.InScope != null)
            foreach (var sp in scopeAndProcedures.InScope)
                inScope.Add("\t" + sp.Component + " & " + sp.Detail + "\u0020\\\\");

        // Todo
        //var worstCaseScenariosReportItems = new List<string>();

        var worstCaseScenariosScope = new List<string>();
        if (scopeAndProcedures.WorstCaseScenarios != null)
            for (var i = 0; i < scopeAndProcedures.WorstCaseScenarios.Count; i++)
            {
                var wcss = scopeAndProcedures.WorstCaseScenarios[i];
                worstCaseScenariosScope.Add("\tWS" + (i + 1) + "\t&\t" + wcss + "\t\\\\");
            }

        var outOfScope = new List<string>();
        if (scopeAndProcedures.OutOfScope != null)
            foreach (var sp in scopeAndProcedures.OutOfScope)
                outOfScope.Add("\t" + sp.Component + " & " + sp.Detail + "\u0020\\\\");

        var environment = new List<string>();
        if (scopeAndProcedures.Environment != null)
            foreach (var env in scopeAndProcedures.Environment)
                environment.Add("\t\t\\item " + env);

        var scopeAndProceduresContent =
            @"%----------------------------------------------------------------------------------------
%	IN SCOPE
%----------------------------------------------------------------------------------------
\newcommand{\InScope}{

	\hline
" + string.Join("\n", inScope) + @"						
}


%----------------------------------------------------------------------------------------
%	WORST CASE SCENARIOS
%----------------------------------------------------------------------------------------

% Not needed for Scope Document
% Required for Report


" + scopeAndProcedures.WorstCaseScenariosReport + @"


% Required for Scope & Report.
\newcommand{\WorstCaseScenariosScope}{
	\hline
" + string.Join("\n", worstCaseScenariosScope) + @"
}



%----------------------------------------------------------------------------------------
%	OUT OF SCOPE
%----------------------------------------------------------------------------------------
\newcommand{\OutOfScope}{
    \hline
" + string.Join("\n", outOfScope) + @"
}

%----------------------------------------------------------------------------------------
%	ENVIRONMENT
%----------------------------------------------------------------------------------------
\newcommand{\Environment}{
    
    \ReportAssessmentTeamLong TODOTODO.
" + (environment.Count >= 1 ? "\t\\begin{itemize}" : null) + @"
" + string.Join(",\n", environment) + @"
" + (environment.Count >= 1 ? "\t\\end{itemize}" : null) + @"
}

%----------------------------------------------------------------------------------------
%	TEST PROTOCOL: TARGET
%----------------------------------------------------------------------------------------
\newcommand{\TargetTestProtocol}{https://TODOTODO}
";
        Console.WriteLine(scopeAndProceduresContent);
        return Encoding.UTF8.GetBytes(scopeAndProceduresContent);
    }
}