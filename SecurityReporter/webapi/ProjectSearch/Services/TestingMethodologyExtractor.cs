using Microsoft.Extensions.ObjectPool;
using System.IO.Compression;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services
{
    public class TestingMethodologyExtractor
    {
        ZipArchiveEntry testMethodEntry;
        TestingMethodology newTestingMethodology = new TestingMethodology();

        public TestingMethodologyExtractor(ZipArchiveEntry testMethodEntry) {
            this.testMethodEntry = testMethodEntry;
            newTestingMethodology.ToolsUsed = new List<Tool>();
            newTestingMethodology.AttackVectors = new List<string>();
        }

        public TestingMethodology ExtractTestingMethodology() {
            string line;
            char[] delimiters = { '{', '}', '&' };
            string[] toolUsedDelimiters = { "&", "\\\\" };
            string[] attackVectorsDelimiters = { "\\item", ",", "{", "}" };

            bool toolsUsed = false;
            bool attackVectors = false;
            if(testMethodEntry == null) {
                throw new ArgumentNullException();
            } else
            {
                using(StreamReader reader = new StreamReader(testMethodEntry.Open()))
                {
                    string[] inBracketContents;
                    while((line = reader.ReadLine()) != null)
                    {
                        if(!string.IsNullOrEmpty(line) && line[0] != '%')
                        {
                            if(!toolsUsed && !attackVectors)
                            {
                                inBracketContents = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                                if(inBracketContents.Length > 1)
                                {
                                    toolsUsed = (inBracketContents[1] == "\\ToolsUsed") ? true : false;
                                    attackVectors = (inBracketContents[1] == "\\AttackVectors") ? true : false;
                                }

                            } else if(toolsUsed && !attackVectors) 
                            {
                                string trimmedLine = line.Trim();
                                toolsUsed = trimmedLine.Contains('}') ? false : true;
                                string[] lineContents = trimmedLine.Split(toolUsedDelimiters, StringSplitOptions.RemoveEmptyEntries);

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
                    }
                    return newTestingMethodology;
                }
            }
        }
    }
}
