using System.Text;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

public static class DbExecutiveSummaryExtractor
{
    public static byte[] ExtractExecutiveSummary(string executiveSummary)
    {
        var executiveSummaryContent =
            @"%----------------------------------------------------------------------------------------
%	EXECUTIVE SUMMARY
%----------------------------------------------------------------------------------------
%-<ExecSum>->

" + executiveSummary + @"

%-<ExecSum>
\pagebreak
\section*{Overall Exposure}";
        Console.WriteLine(executiveSummaryContent);
        return Encoding.UTF8.GetBytes(executiveSummaryContent);
    }
}