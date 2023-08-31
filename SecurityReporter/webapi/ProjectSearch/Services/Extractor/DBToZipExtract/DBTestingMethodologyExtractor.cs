using System.Text;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

public static class DbTestingMethodologyExtractor
{
    public static byte[] ExtractTestingMethodology(TestingMethodology testingMethodology)
    {

        //var toolsUsed = new List<string>();
        /*(if (testingMethodology.ToolsUsed != null) {
            foreach(Tool tool in testingMethodology.ToolsUsed)
            {
                toolsUsed.Add("\t" + tool.ToolName + " & " + tool.Version + " & " + tool.TestType + " & " + tool.WorkType + " \\\\\n\t\\hline");
            }
        }

        var attackVectors = new List<string>();
        if (testingMethodology.AttackVectors != null)
        {
            foreach(string attackVector in testingMethodology.AttackVectors)
            {
                attackVectors.Add("\t\t\\item " + attackVector);
            }
        }*/

        var testingMethodologyContent =
            @"%----------------------------------------------------------------------------------------
%	TOOLS USED
%----------------------------------------------------------------------------------------
% Not needed for Scope document
% Required for Report document
\newcommand{\ToolsUsed}{
" + testingMethodology.ToolsUsed + @"
} 


%----------------------------------------------------------------------------------------
%	ATTACK VECTORS AND PAYLOAD TYPES
%----------------------------------------------------------------------------------------
% Not needed for Scope document
% Required for Report document
\newcommand{\AttackVectors}{
" + testingMethodology.AttackVectors + @"


}";
        Console.WriteLine(testingMethodologyContent);
        return Encoding.UTF8.GetBytes(testingMethodologyContent);
    }
}
