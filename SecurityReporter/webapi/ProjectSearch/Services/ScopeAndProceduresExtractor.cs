using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO.Compression;
using webapi.Models.ProjectReport;

namespace webapi.ProjectSearch.Services
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
            if(currentEntry == null)
            {
                throw new ArgumentNullException();
            } else
            {
                using(StreamReader reader = new StreamReader(currentEntry.Open()))
                {
                    char[] delimiters = { '{', '}' };
                    while((line = reader.ReadLine()) != null)
                    {
                        if(!string.IsNullOrEmpty(line) && line.Length > 0) 
                        {
                            string[] splitLine = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if(splitLine.Length >= 2) 
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
                                }
                                else if (splitLine[1] == "\\WorstCaseScenariosScope")
                                {
                                    extractScopeAndWorstCase(false, true, reader, newScopeAndProcedures);
                                }
                                else if (splitLine[1] == "\\Environment")
                                {

                                }
                            }
                        }
                    }
                }
                return newScopeAndProcedures;
            }
        }

        private void extractScopeAndWorstCase(bool inScope, bool worstCase, StreamReader reader, ScopeAndProcedures newScopeAndProcedures)
        {
            List<ScopeProcedure> newScopeProcedureList = new List<ScopeProcedure>();
            string line;
            string[] delimiters = { "&", "\\\\" };
            bool read = true;

            while((line = reader.ReadLine()) != null && read) {
                if(!string.IsNullOrEmpty(line) && line.Length > 0)
                {
                    string trimmedLine = line.Trim();
                     if (trimmedLine[0] == '}')
                     {
                        if(newScopeProcedureList.Count() != 0)
                        {
                            if(inScope && !worstCase)
                            {
                                newScopeAndProcedures.InScope = newScopeProcedureList;
                            } else if(!inScope && !worstCase) 
                            {
                                newScopeAndProcedures.OutOfScope = newScopeProcedureList;
                            } else if(!inScope && worstCase)
                            {
                                List<string> newWorstCaseList = new List<string>();
                                foreach(ScopeProcedure procedure in newScopeProcedureList)
                                {
                                    newWorstCaseList.Add(procedure.Detail);
                                }

                                newScopeAndProcedures.WorstCaseScenarios = newWorstCaseList;
                            }
                        }
                        read = false;
                    } else if (trimmedLine[0] != '\\')
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

        private List<string> extractWorstCase()
        {
            return null;
        }

        private void extractWorstCaseReport()
        {

        }

        private List<string> exportEnvironment()
        {
            return null;
        }
    }
}
