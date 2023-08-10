using System.IO.Compression;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract
{
    public class ScopeAndProceduresExtractor
    {
        private ZipArchiveEntry currentEntry;
        private ScopeAndProcedures newScopeAndProcedures = new ScopeAndProcedures();
        public ScopeAndProceduresExtractor(ZipArchiveEntry currentEntry)
        {
            this.currentEntry = currentEntry;
        }

        public ScopeAndProcedures ExtractScopeAndProcedures()
        {
            string line;
            if (currentEntry == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                using (StreamReader reader = new StreamReader(currentEntry.Open()))
                {
                    char[] delimiters = { '{', '}' };
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line) && line.Length > 0)
                        {
                            string[] splitLine = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (splitLine.Length >= 2)
                            {
                                if (splitLine[1] == "\\InScope")
                                {
                                    extractScopeAndWorstCase(true, false, reader, newScopeAndProcedures);
                                }
                                else if (splitLine[1] == "\\OutOfScope")
                                {
                                    extractScopeAndWorstCase(false, false, reader, newScopeAndProcedures);
                                }
                                else if (splitLine[1] == "\\WorstCaseScenariosReport")
                                {
                                    extractWorstCaseReport(reader, newScopeAndProcedures);
                                }
                                else if (splitLine[1] == "\\WorstCaseScenariosScope")
                                {
                                    extractScopeAndWorstCase(false, true, reader, newScopeAndProcedures);
                                }
                                else if (splitLine[1] == "\\Environment")
                                {
                                    extractEnvironment(reader, newScopeAndProcedures);
                                }
                            }
                        }
                    }
                }
                return newScopeAndProcedures;
            }
        }

        private void extractScope(string command, string contents)
        {
            string[] splitString = contents.Split('\\', StringSplitOptions.RemoveEmptyEntries);
            string[] componentSplit;
            foreach(string splitLine in splitString)
            {
                componentSplit = splitLine.Split('&', StringSplitOptions.RemoveEmptyEntries);
                if (componentSplit.Length >= 2)
                {
                    ScopeProcedure scopeProcedure = new ScopeProcedure();
                    scopeProcedure.Component = componentSplit[0];
                    scopeProcedure.Detail = componentSplit[1];
                    switch (command)
                    {
                        case "InScope":
                            newScopeAndProcedures.InScope.Add(scopeProcedure);
                            break;
                        case "WorstCaseScenariosScope":
                            newScopeAndProcedures.WorstCaseScenarios.Add(componentSplit[1]);
                            break;
                        case "OutOfScope":
                            newScopeAndProcedures.OutOfScope.Add(scopeProcedure);
                            break;
                    }
                }
            }
        }

        private void extractScopeAndWorstCase(bool inScope, bool worstCase, StreamReader reader, ScopeAndProcedures newScopeAndProcedures)
        {
            List<ScopeProcedure> newScopeProcedureList = new List<ScopeProcedure>();
            string line;
            string[] delimiters = { "&", "\\\\" };
            bool read = true;

            while ((line = reader.ReadLine()) != null && read)
            {
                if (!string.IsNullOrEmpty(line) && line.Length > 0)
                {
                    string trimmedLine = line.Trim();
                    if (trimmedLine[0] == '}')
                    {
                        if (newScopeProcedureList.Count() != 0)
                        {
                            if (inScope && !worstCase)
                            {
                                newScopeAndProcedures.InScope = newScopeProcedureList;
                            }
                            else if (!inScope && !worstCase)
                            {
                                newScopeAndProcedures.OutOfScope = newScopeProcedureList;
                            }
                            else if (!inScope && worstCase)
                            {
                                List<string> newWorstCaseList = new List<string>();
                                foreach (ScopeProcedure procedure in newScopeProcedureList)
                                {
                                    newWorstCaseList.Add(procedure.Detail);
                                }

                                newScopeAndProcedures.WorstCaseScenarios = newWorstCaseList;
                            }
                        }
                        read = false;
                    }
                    else if (trimmedLine[0] != '\\')
                    {
                        string[] splitLine = trimmedLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        if (splitLine.Length == 2)
                        {
                            ScopeProcedure newScopeProcedure = new ScopeProcedure();
                            newScopeProcedure.Component = splitLine[0].Trim();
                            newScopeProcedure.Detail = splitLine[1].Trim();

                            newScopeProcedureList.Add(newScopeProcedure);
                        }
                    }
                }
            }
        }
        private void extractWorstCaseReport(StreamReader reader, ScopeAndProcedures newScopeAndProcedures)
        {
            string line;
            bool read = true;
            string newReport = "\\newcommand{\\WorstCaseScenariosReport}{\n";

            while ((line = reader.ReadLine()) != null && read)
            {
                newReport += (line + "\n");
                if (!string.IsNullOrEmpty(line) && line.Length > 0)
                {
                    string trimmedLine = line.Trim();
                    if (trimmedLine.Length >= 15)
                    {
                        read = trimmedLine == "\\end{xltabular}" ? false : true;
                    }
                }
            }
            newReport += "\n}";
            newScopeAndProcedures.WorstCaseScenariosReport = newReport;
        }

        private void extractEnvironment(StreamReader reader, ScopeAndProcedures newScopeAndProcedures)
        {
            string line;
            bool read = true;
            bool readItem = false;
            string[] delimiters = { "\\item", "," };
            List<string> newEnvironment = new List<string>();

            while ((line = reader.ReadLine()) != null && read)
            {
                if (!string.IsNullOrEmpty(line) && line.Length > 0)
                {
                    string trimmedLine = line.Trim();
                    if (trimmedLine.Length >= 15 && !readItem)
                    {
                        readItem = trimmedLine == "\\begin{itemize}" ? true : false;
                    }
                    else if (trimmedLine.Length >= 12 && readItem)
                    {
                        if (trimmedLine == "\\end{itemize}")
                        {
                            readItem = false;
                            read = false;
                        }
                    }
                    if (readItem)
                    {
                        if (trimmedLine.Substring(0, 5) == "\\item")
                        {
                            string[] splitString = trimmedLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (splitString.Length > 0)
                            {
                                newEnvironment.Add(splitString[0].Trim());
                            }
                        }
                    }
                }
            }
            if (newEnvironment.Count > 0)
            {
                newScopeAndProcedures.Environment = newEnvironment;
            }
        }
    }
}
