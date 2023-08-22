using System.IO.Compression;
using webapi.ProjectSearch.Models.ProjectReport;

namespace webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

public class ScopeAndProceduresExtractor
{
    private readonly ZipArchiveEntry currentEntry;
    private readonly ScopeAndProcedures newScopeAndProcedures = new();

    public ScopeAndProceduresExtractor(ZipArchiveEntry currentEntry)
    {
        this.currentEntry = currentEntry;
    }

    public ScopeAndProcedures ExtractScopeAndProcedures()
    {
        string line;
        if (currentEntry == null) throw new ArgumentNullException();

        using (var reader = new StreamReader(currentEntry.Open()))
        {
            char[] delimiters = { '{', '}' };
            while ((line = reader.ReadLine()) != null)
                if (!string.IsNullOrEmpty(line) && line.Length > 0)
                {
                    var splitLine = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (splitLine.Length >= 2)
                    {
                        if (splitLine[1] == "\\InScope")
                            ExtractScopeAndWorstCase(true, false, reader, newScopeAndProcedures);
                        else if (splitLine[1] == "\\OutOfScope")
                            ExtractScopeAndWorstCase(false, false, reader, newScopeAndProcedures);
                        else if (splitLine[1] == "\\WorstCaseScenariosReport")
                            ExtractWorstCaseReport(reader, newScopeAndProcedures);
                        else if (splitLine[1] == "\\WorstCaseScenariosScope")
                            ExtractScopeAndWorstCase(false, true, reader, newScopeAndProcedures);
                        else if (splitLine[1] == "\\Environment") ExtractEnvironment(reader, newScopeAndProcedures);
                    }
                }
        }

        return newScopeAndProcedures;
    }

    private void ExtractScope(string command, string contents)
    {
        var splitString = contents.Split('\\', StringSplitOptions.RemoveEmptyEntries);
        string[] componentSplit;
        foreach (var splitLine in splitString)
        {
            componentSplit = splitLine.Split('&', StringSplitOptions.RemoveEmptyEntries);
            if (componentSplit.Length >= 2)
            {
                var scopeProcedure = new ScopeProcedure();
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

    private static void ExtractScopeAndWorstCase(bool inScope, bool worstCase, StreamReader reader,
        ScopeAndProcedures newScopeAndProcedures)
    {
        var newScopeProcedureList = new List<ScopeProcedure>();
        string line;
        string[] delimiters = { "&", "\\\\" };
        var read = true;

        while ((line = reader.ReadLine()) != null && read)
            if (!string.IsNullOrEmpty(line) && line.Length > 0)
            {
                var trimmedLine = line.Trim();
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
                            var newWorstCaseList = new List<string>();
                            foreach (var procedure in newScopeProcedureList) newWorstCaseList.Add(procedure.Detail);

                            newScopeAndProcedures.WorstCaseScenarios = newWorstCaseList;
                        }
                    }

                    read = false;
                }
                else if (trimmedLine[0] != '\\')
                {
                    var splitLine = trimmedLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (splitLine.Length == 2)
                    {
                        var newScopeProcedure = new ScopeProcedure();
                        newScopeProcedure.Component = splitLine[0].Trim();
                        newScopeProcedure.Detail = splitLine[1].Trim();

                        newScopeProcedureList.Add(newScopeProcedure);
                    }
                }
            }
    }

    private static void ExtractWorstCaseReport(StreamReader reader, ScopeAndProcedures newScopeAndProcedures)
    {
        string line;
        var read = true;
        var newReport = "\\newcommand{\\WorstCaseScenariosReport}{\n";

        while ((line = reader.ReadLine()) != null && read)
        {
            newReport += line + "\n";
            if (!string.IsNullOrEmpty(line) && line.Length > 0)
            {
                var trimmedLine = line.Trim();
                if (trimmedLine.Length >= 15) read = trimmedLine == "\\end{xltabular}" ? false : true;
            }
        }

        newReport += "\n}";
        newScopeAndProcedures.WorstCaseScenariosReport = newReport;
    }

    private static void ExtractEnvironment(StreamReader reader, ScopeAndProcedures newScopeAndProcedures)
    {
        string line;
        var read = true;
        var readItem = false;
        string[] delimiters = { "\\item", "," };
        var newEnvironment = new List<string>();

        while ((line = reader.ReadLine()) != null && read)
            if (!string.IsNullOrEmpty(line) && line.Length > 0)
            {
                var trimmedLine = line.Trim();
                if (trimmedLine.Length >= 15 && !readItem)
                    readItem = trimmedLine == "\\begin{itemize}" ? true : false;
                else if (trimmedLine.Length >= 12 && readItem)
                    if (trimmedLine == "\\end{itemize}")
                    {
                        readItem = false;
                        read = false;
                    }

                if (readItem)
                    if (trimmedLine.Substring(0, 5) == "\\item")
                    {
                        var splitString = trimmedLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        if (splitString.Length > 0) newEnvironment.Add(splitString[0].Trim());
                    }
            }

        if (newEnvironment.Count > 0) newScopeAndProcedures.Environment = newEnvironment;
    }
}