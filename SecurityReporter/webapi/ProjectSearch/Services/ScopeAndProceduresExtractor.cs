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
        private void extractWorstCaseReport(StreamReader reader, ScopeAndProcedures newScopeAndProcedures)
        {
            string line;
            bool read = true;
            string[] delimiters = { "&", "\\Huge", "\\\\"};
            WorstCaseScenarioReport newReport = new WorstCaseScenarioReport();

            while ((line = reader.ReadLine()) != null && read)
            {
                if(!string.IsNullOrEmpty(line) && line.Length > 0) 
                { 
                    string trimmedLine = line.Trim();
                    if(trimmedLine.Length >= 15) 
                    {
                        read = (trimmedLine == "\\end{xltabular}") ? false : true;
                    }

                    string[] splitString = trimmedLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    splitString = splitString.Where(item => !string.IsNullOrWhiteSpace(item)).ToArray();
                    
                    if(splitString.Length == 6 && splitString[0][0] != '\\') 
                    {
                        List<bool> newBoolList = new List<bool>();

                        for (int i = 0; i < splitString.Length; i++)
                        {

                            switch(i)
                            {
                                case 1:
                                    newReport.FindingDescription = splitString[i].Trim();
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                    string trimSplitString = splitString[i].Trim();
                                    if(trimSplitString == "$\\cdotp$")
                                    {
                                        newBoolList.Add(true);
                                    } else if(trimSplitString == "\\phantom{}")
                                    {
                                        newBoolList.Add(false);
                                    }
                                    break;
                            }
                        }
                        newReport.WorstCaseReport.Add(newBoolList);
                        newScopeAndProcedures.WorstCaseScenariosReport = newReport;
                    }
                }
            }
        }

        private void extractEnvironment(StreamReader reader, ScopeAndProcedures newScopeAndProcedures)
        {
            string line;
            bool read = true;
            bool readItem = false;
            string[] delimiters = { "\\item", "," };
            List<string > newEnvironment = new List<string>();
            
            while((line = reader.ReadLine()) != null && read) 
            { 
                if(!string.IsNullOrEmpty(line) && line.Length > 0)
                {
                    string trimmedLine = line.Trim();
                    if(trimmedLine.Length >= 15 && !readItem) {
                        readItem = (trimmedLine == "\\begin{itemize}") ? true : false;
                    } else if(trimmedLine.Length >= 12 && readItem)
                    {
                        if(trimmedLine == "\\end{itemize}")
                        {
                            readItem = false;
                            read = false;
                        }
                    } 
                    if(readItem)
                    {
                        if(trimmedLine.Substring(0,5) == "\\item")
                        {
                            string[] splitString = trimmedLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if(splitString.Length > 0)
                            {
                                newEnvironment.Add(splitString[0].Trim());
                            }
                        }
                    }
                }
            }
            if(newEnvironment.Count > 0)
            {
                newScopeAndProcedures.Environment = newEnvironment;
            }
        }
    }
}
