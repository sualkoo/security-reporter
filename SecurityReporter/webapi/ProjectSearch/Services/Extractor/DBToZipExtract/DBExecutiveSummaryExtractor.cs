using System.Text;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract
{
    public class DbExecutiveSummaryExtractor
    {
        public static byte[] ExtractExecutiveSummary(string executiveSummary)
        {
            string executiveSummaryContent =
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
}
