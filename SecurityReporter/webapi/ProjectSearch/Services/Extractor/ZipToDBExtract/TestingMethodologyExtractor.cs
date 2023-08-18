using System.IO.Compression;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

public class TestingMethodologyExtractor
{
    private readonly TestingMethodology newTestingMethodology = new();
    private readonly ZipArchiveEntry testMethodEntry;

    public TestingMethodologyExtractor(ZipArchiveEntry testMethodEntry)
    {
        this.testMethodEntry = testMethodEntry;
        newTestingMethodology.ToolsUsed = new List<Tool>();
        newTestingMethodology.AttackVectors = new List<string>();
    }

    public TestingMethodology ExtractTestingMethodology()
    {
        string line;
        char[] delimiters = { '{', '}', '&' };
        string[] toolUsedDelimiters = { "&", "\\\\" };
        string[] attackVectorsDelimiters = { "\\item", ",", "{", "}" };

        var toolsUsed = false;
        var attackVectors = false;
        if (testMethodEntry == null)
            throw new ArgumentNullException();
        using (var reader = new StreamReader(testMethodEntry.Open()))
        {
            string[] inBracketContents;
            while ((line = reader.ReadLine()) != null)
                if (!string.IsNullOrEmpty(line) && line[0] != '%')
                {
                    if (!toolsUsed && !attackVectors)
                    {
                        inBracketContents = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                        if (inBracketContents.Length > 1)
                        {
                            toolsUsed = inBracketContents[1] == "\\ToolsUsed" ? true : false;
                            attackVectors = inBracketContents[1] == "\\AttackVectors" ? true : false;
                        }
                    }
                    else if (toolsUsed && !attackVectors)
                    {
                        var trimmedLine = line.Trim();
                        toolsUsed = trimmedLine.Contains('}') ? false : true;
                        var lineContents = trimmedLine.Split(toolUsedDelimiters, StringSplitOptions.RemoveEmptyEntries);

                        if (toolsUsed)
                            if (lineContents.Length == 4)
                            {
                                var newTool = new Tool();
                                newTool.ToolName = lineContents[0].Trim();
                                newTool.Version = lineContents[1].Trim();
                                newTool.TestType = lineContents[2].Trim();
                                newTool.WorkType = lineContents[3].Trim();
                                newTestingMethodology.ToolsUsed.Add(newTool);
                            }
                    }
                    else if (!toolsUsed && attackVectors)
                    {
                        var trimmedLine = line.Trim();
                        attackVectors = trimmedLine == "\\end{itemize}" ? false : true;
                        if (attackVectors)
                        {
                            var lineContents = trimmedLine.Split(attackVectorsDelimiters,
                                StringSplitOptions.RemoveEmptyEntries);
                            if (lineContents.Length == 1)
                                newTestingMethodology.AttackVectors.Add(lineContents[0].Trim());
                        }
                    }
                    else
                    {
                        throw new Exception("The LaTeX file is not formatted as required");
                    }
                }

            if (toolsUsed || attackVectors) throw new Exception("The LaTeX file is not formatted as required");
            return newTestingMethodology;
        }
    }
}