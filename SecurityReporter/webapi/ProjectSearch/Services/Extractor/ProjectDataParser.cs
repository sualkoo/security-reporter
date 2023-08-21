using System.IO.Compression;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Models.ProjectReport;
using webapi.ProjectSearch.Services.Extractor.ZipToDBExtract;

namespace webapi.ProjectSearch.Services.Extractor;

public class ProjectDataParser : IProjectDataParser
{
    private readonly ILogger logger;

    public ProjectDataParser()
    {
        var loggerFactory = LoggerProvider.GetLoggerFactory();
        logger = loggerFactory.CreateLogger<ProjectDataParser>();
    }

    public ProjectReportData Extract(Stream zipStream)
    {
        var newProjectReportData = new ProjectReportData();

        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
        {
            if (zipStream != null)
            {
                var currentEntry = archive.GetEntry("Static/PentestTeam.tex");
                var pte = new PentestTeamExtractor(currentEntry);
                var pentestTeamDict = pte.ExtractPentestTeam();
                currentEntry = archive.GetEntry("Config/Document_Information.tex");
                var die = new DocumentInformationExtractor(currentEntry);
                newProjectReportData.DocumentInfo = die.ExtractDocumentInformation();
                currentEntry = archive.GetEntry("Config/Executive_Summary.tex");
                var ese = new ExecutiveSummaryExtractor(currentEntry);
                newProjectReportData.ExecutiveSummary = ese.ExtractExecutiveSummary();
                currentEntry = archive.GetEntry("Config/Project_Information.tex");
                var pie = new ProjectInformationExtractor(currentEntry, pentestTeamDict);
                newProjectReportData.ProjectInfo = pie.ExtractProjectInformation();
                currentEntry = archive.GetEntry("Config/Scope_and_Procedures.tex");
                var sape = new ScopeAndProceduresExtractor(currentEntry);
                newProjectReportData.ScopeAndProcedures = sape.ExtractScopeAndProcedures();
                currentEntry = archive.GetEntry("Config/Testing_Methodology.tex");
                var tme = new TestingMethodologyExtractor(currentEntry);
                newProjectReportData.TestingMethodology = tme.ExtractTestingMethodology();
                newProjectReportData.Findings = ExtractFindings(archive);
                currentEntry = archive.GetEntry("Config/Findings_Database/Findings_Database.tex");
                if (currentEntry != null)
                    using (var reader = new StreamReader(currentEntry.Open()))
                    {
                        newProjectReportData.FindingsDatabase = reader.ReadToEnd();
                    }

                newProjectReportData.uploadDate = DateTime.Now;
            }
            else if (zipStream == null)
            {
                throw new ArgumentNullException(nameof(zipStream));
            }
            else if (archive == null)
            {
                throw new ArgumentNullException(nameof(archive));
            }
        }

        logger.LogInformation("Successfully extracted data from zip file");
        return newProjectReportData;
    }

    public List<Finding> ExtractFindings(ZipArchive archive)
    {
        var fe = new FindingsExtractor();
        var findingDictionary = new Dictionary<string, List<ZipArchiveEntry>>();
        var findingsList = new List<Finding>();
        if (archive == null)
        {
            throw new ArgumentNullException();
        }

        foreach (var fileEntry in archive.Entries)
            if (fileEntry.FullName.StartsWith("Config/Findings_Database")
                && !fileEntry.FullName.StartsWith("Config/Findings_Database/DR_Template"))
            {
                var splitString = fileEntry.FullName.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var folderPath = "";
                if (splitString.Length > 3) folderPath = splitString[0] + "/" + splitString[1] + "/" + splitString[2];
                if (!string.IsNullOrEmpty(folderPath))
                {
                    if (!findingDictionary.ContainsKey(folderPath))
                        findingDictionary.Add(folderPath, new List<ZipArchiveEntry> { fileEntry });
                    else
                        findingDictionary[folderPath].Add(fileEntry);
                }
            }

        foreach (var list in findingDictionary.Values)
        {
            Finding newFinding = null;
            var processedMembers = new List<ZipArchiveEntry>();

            for (var count = 0; count < list.Count;)
                foreach (var entry in list)
                    if (count == 0)
                    {
                        if (entry.FullName.EndsWith("main.tex"))
                        {
                            newFinding = fe.ExtractFinding(entry);
                            processedMembers.Add(entry);
                            var splitString = entry.FullName.Split('/', StringSplitOptions.RemoveEmptyEntries);
                            newFinding.FolderName = splitString[2];
                            count++;
                        }
                    }
                    else
                    {
                        if (!processedMembers.Contains(entry))
                        {
                            newFinding.GetImages().Add(ProcessImages(entry));
                            processedMembers.Add(entry);
                            count++;
                        }
                    }

            findingsList.Add(newFinding);
        }

        return findingsList;
    }

    private FileData ProcessImages(ZipArchiveEntry image)
    {
        var splitString = image.FullName.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var fileName = "";
        if (splitString.Length > 3) fileName = splitString[3];

        byte[] contents;
        using (var entryStream = image.Open())
        using (var memoryStream = new MemoryStream())
        {
            entryStream.CopyTo(memoryStream);
            contents = memoryStream.ToArray();
        }

        var newData = new FileData();
        newData.FileName = fileName;
        newData.Content = contents;

        return newData;
    }
}