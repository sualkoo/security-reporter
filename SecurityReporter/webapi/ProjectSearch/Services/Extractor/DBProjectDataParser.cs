using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

namespace webapi.ProjectSearch.Services.Extractor;

public class DbProjectDataParser : IDbProjectDataParser
{
    private readonly ILogger logger;

    public DbProjectDataParser(ILogger<DbProjectDataParser> logger)
    {
        this.logger = logger;
    }

    public FileContentResult Extract(ProjectReportData data)
    {
        if (data == null || data.DocumentInfo == null || data.ProjectInfo == null || data.ExecutiveSummary == null ||
            data.ScopeAndProcedures == null || data.TestingMethodology == null)
            throw new CustomException(StatusCodes.Status500InternalServerError,
                "Looks like we have missing/incorrect information in project report data.");
        logger.LogInformation("Fetched data: " + (data != null ? data.DocumentInfo!.ProjectReportName : null));
        logger.LogInformation("Trying to parse data");

            try
            {
                var fileContents = new Dictionary<string, byte[]>()
                {
                    { "Config/Document_Information.tex", DbDocumentInformationExtractor.ExtractDocumentInformation(data!.DocumentInfo) },
                    { "Config/Executive_Summary.tex", DbExecutiveSummaryExtractor.ExtractExecutiveSummary(data!.ExecutiveSummary) },
                    { "Config/Project_Information.tex", DbProjectInformationExtractor.ExtractProjectInformation(data!.ProjectInfo) },
                    { "Config/Scope_and_Procedures.tex", DbScopeAndProceduresExtractor.ExtractScopeAndProcedures(data!.ScopeAndProcedures) },
                    { "Config/Testing_Methodology.tex", DbTestingMethodologyExtractor.ExtractTestingMethodology(data!.TestingMethodology) },
                };

            var zipFileBytes = CreateZipFile(fileContents, data);

            return new FileContentResult(zipFileBytes, "application/zip")
            {
                FileDownloadName = !string.IsNullOrEmpty(data.DocumentInfo.ProjectReportName)
                    ? "Report-" + data.DocumentInfo.ProjectReportName!.Replace(" ", "_")
                    : "Project-Unnamed" // Provide a desired name for the downloaded zip file.
            };
        }
        catch (Exception)
        {
            throw new CustomException(StatusCodes.Status500InternalServerError,
                "An error occured during parsing data from DB.",
                new List<string> { "Config files cannot be created due to incorrect data/other problem." });
        }
    }

    // Helper function
    private byte[] CreateZipFile(Dictionary<string, byte[]> fileContents, ProjectReportData projectReportData)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                using (var templateZip =
                       ZipFile.OpenRead("./ProjectSearch/Services/Extractor/DBToZipExtract/Base_Template.zip"))
                {
                    foreach (var templateEntry in templateZip.Entries)
                    {
                        var destinationEntry = zipArchive.CreateEntry(templateEntry.FullName);
                        using (var sourceStream = templateEntry.Open())
                        using (var destinationStream = destinationEntry.Open())
                        {
                            sourceStream.CopyTo(destinationStream);
                        }
                    }
                }

                foreach (var entry in fileContents)
                {
                    var zipEntry = zipArchive.CreateEntry(entry.Key);
                    using (var entryStream = zipEntry.Open())
                    {
                        entryStream.Write(entry.Value, 0, entry.Value.Length);
                    }
                }

                ExtractFindings(zipArchive, projectReportData);
            }

            return memoryStream.ToArray();
        }
    }

    private void ExtractFindings(ZipArchive zipArchive, ProjectReportData projectReportData)
    {
        foreach (var finding in projectReportData.Findings)
        {
            var resultFinding = DbFindingsExtractor.ExtractFinding(finding);
            var entryFolderName = "Config/Findings_Database/" + finding.FolderName;

            var mainFile = zipArchive.CreateEntry(entryFolderName + "/main.tex");
            using (var entryStream = mainFile.Open())
            {
                resultFinding.Item1.CopyTo(entryStream);
            }

            foreach (var imageData in resultFinding.Item2)
            {
                var imageEntryName = entryFolderName + "/" + imageData.FileName;
                var entry = zipArchive.CreateEntry(imageEntryName);
                using (var entryStream = entry.Open())
                {
                    entryStream.Write(imageData.image, 0, imageData.image.Length);
                }
            }
        }

        var newEntry = zipArchive.CreateEntry("Config/Findings_Database/Findings_Database.tex");
        using (var writer = new StreamWriter(newEntry.Open()))
        {
            writer.Write(projectReportData.FindingsDatabase);
        }
    }
}