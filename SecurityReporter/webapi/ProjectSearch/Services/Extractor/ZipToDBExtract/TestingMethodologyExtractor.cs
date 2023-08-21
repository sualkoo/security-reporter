using System.IO.Compression;
using System.Text.RegularExpressions;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

public class TestingMethodologyExtractor
{
    private readonly TestingMethodology newTestingMethodology = new();
    private readonly ZipArchiveEntry testMethodEntry;

    public TestingMethodologyExtractor(ZipArchiveEntry testMethodEntry)
    {
        this.testMethodEntry = testMethodEntry;
        //newTestingMethodology.ToolsUsed = new List<Tool>();
        //newTestingMethodology.AttackVectors = new List<string>();
    }

    public TestingMethodology ExtractTestingMethodology()
    {
        string line;
        char[] delimiters = { '{', '}', '&' };
        string[] toolUsedDelimiters = { "&", "\\\\" };
        string[] attackVectorsDelimiters = { "\\item", ",", "{", "}" };
        string commandRegex = @"\\newcommand{\\([\S]*)}\s*{([\s\S]*?)\n\s*}";


        bool toolsUsed = false;
        bool attackVectors = false;
        if (testMethodEntry == null)
        {
            throw new ArgumentNullException();
        }
        else
        {
            using (StreamReader reader = new StreamReader(testMethodEntry.Open()))
            {
                string fileContents = reader.ReadToEnd();
                MatchCollection matches = Regex.Matches(fileContents, commandRegex);

                foreach (Match match in matches)
                {
                    AssignNewData(match.Groups[1].ToString(), match.Groups[2].ToString());
                }

                /*string[] inBracketContents;
                while((line = reader.ReadLine()) != null)
                {
                    if(!string.IsNullOrEmpty(line) && line[0] != '%')
                    {
                        if(!toolsUsed && !attackVectors)
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

                            if(toolsUsed)
                            {
                                if (lineContents.Length == 4)
                                {
                                    Tool newTool = new Tool();
                                    newTool.ToolName = lineContents[0].Trim();
                                    newTool.Version = lineContents[1].Trim();
                                    newTool.TestType = lineContents[2].Trim();
                                    newTool.WorkType = lineContents[3].Trim();
                                    newTestingMethodology.ToolsUsed.Add(newTool);
                                }
                            }

                        } else if(!toolsUsed && attackVectors)
                        {
                            string trimmedLine = line.Trim();
                            attackVectors = (trimmedLine == ("\\end{itemize}")) ? false : true;
                            if(attackVectors)
                            {
                                string[] lineContents = trimmedLine.Split(attackVectorsDelimiters, StringSplitOptions.RemoveEmptyEntries);
                                if (lineContents.Length == 1)
                                {
                                    newTestingMethodology.AttackVectors.Add(lineContents[0].Trim());
                                }
                            }
                        } else
                        {
                            throw new Exception("The LaTeX file is not formatted as required");
                        } 
                    }
                }
                if(toolsUsed == true || attackVectors == true)
                {
                    throw new Exception("The LaTeX file is not formatted as required");
                }*/
                return newTestingMethodology;
            }
        }
    }
    private void AssignNewData(string command, string contents)
    {
        switch (command)
        {
            case "ToolsUsed":
                newTestingMethodology.ToolsUsed = contents;
                break;
            case "AttackVectors":
                newTestingMethodology.AttackVectors = contents;
                break;
        }
    }
}

