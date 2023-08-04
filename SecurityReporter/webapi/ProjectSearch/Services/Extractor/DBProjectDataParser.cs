using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;

namespace webapi.ProjectSearch.Services.Extractor
{
    public class DBProjectDataParser : IDBProjectDataParser
    {
        private ILogger Logger;
        DBDocumentInformationExtractor documentInformationExtractor;
        DBExecutiveSummaryExtractor executiveSummaryExtractor;
        DBProjectInformationExtractor projectInformationExtractor;
        DBFindingsExtractor findingsExtractor;
        DBScopeAndProceduresExtractor scopeAndProceduresExtractor;
        DBTestingMethodologyExtractor testingMethodologyExtractor;

        public DBProjectDataParser(ILogger<DBProjectDataParser> logger)
        {
            Logger = logger;
            documentInformationExtractor = new DBDocumentInformationExtractor();
            executiveSummaryExtractor = new DBExecutiveSummaryExtractor();
            projectInformationExtractor = new DBProjectInformationExtractor();
            findingsExtractor = new DBFindingsExtractor();
            scopeAndProceduresExtractor = new DBScopeAndProceduresExtractor();
            testingMethodologyExtractor = new DBTestingMethodologyExtractor();
        }

        public FileContentResult Extract(ProjectReportData data)
        {
            if (data == null || data.DocumentInfo == null || data.ProjectInfo == null || data.ExecutiveSummary == null || data.ScopeAndProcedures == null || data.TestingMethodology == null)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "Looks like we have missing/incorrect information in project report data.");
            }
            Logger.LogInformation("Fetched data: " + (data != null ? data.DocumentInfo!.ProjectReportName : null));
            Logger.LogInformation("Trying to parse data");

            try
            {
                var fileContents = new Dictionary<string, byte[]>()
                {
                    { "Config/Document_Information.tex", documentInformationExtractor.extractDocumentInformation(data!.DocumentInfo) },
                    { "Config/Executive_Summary.tex", executiveSummaryExtractor.extractExecutiveSummary(data!.ExecutiveSummary) },
                    { "Config/Project_Information.tex", projectInformationExtractor.extractProjectInformation(data!.ProjectInfo) },
                    { "Config/Scope_and_Procedures.tex", scopeAndProceduresExtractor.extractScopeAndProcedures(data!.ScopeAndProcedures) },
                    { "Config/Testing_Methodology.tex", testingMethodologyExtractor.extractTestingMethodology(data!.TestingMethodology) },
                };

                byte[] zipFileBytes = CreateZipFile(fileContents, data);

                return new FileContentResult(zipFileBytes, "application/zip")
                {
                    FileDownloadName = (!string.IsNullOrEmpty(data.DocumentInfo.ProjectReportName) ? "Report-" + data.DocumentInfo.ProjectReportName!.Replace(" ","_") : "Project-Unnamed") // Provide a desired name for the downloaded zip file.
                };
            } catch (Exception)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "An error occured during parsing data from DB.", new List<string>() { "Config files cannot be created due to incorrect data/other problem." });
            }

        }

        // Helper function
        private byte[] CreateZipFile(Dictionary<string, byte[]> fileContents, ProjectReportData projectReportData)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    using(ZipArchive TemplateZip = ZipFile.OpenRead("./ProjectSearch/Services/Extractor/DBToZipExtract/Base_Template.zip"))
                    {
                        foreach(ZipArchiveEntry templateEntry in TemplateZip.Entries)
                        {
                            ZipArchiveEntry destinationEntry = zipArchive.CreateEntry(templateEntry.FullName);
                            using(Stream sourceStream = templateEntry.Open())
                            using(Stream destinationStream = destinationEntry.Open())
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

                    extractFindings(zipArchive, projectReportData);
                    
                }
                return memoryStream.ToArray();
            }
        }

        private void extractFindings(ZipArchive zipArchive, ProjectReportData projectReportData)
        {
            foreach (Finding finding in projectReportData.Findings)
            {
                var resultFinding = findingsExtractor.extractFinding(finding);
                string entryFolderName = "Config/Findings_Database/" + finding.FolderName;

                var mainFile = zipArchive.CreateEntry(entryFolderName + "/main.tex");
                using (Stream entryStream = mainFile.Open())
                {
                    resultFinding.Item1.CopyTo(entryStream);
                }

                foreach (var imageData in resultFinding.Item2)
                {
                    string imageEntryName = Path.Combine(entryFolderName, imageData.FileName);
                    ZipArchiveEntry entry = zipArchive.CreateEntry(imageEntryName);
                    using (Stream entryStream = entry.Open())
                    {
                        entryStream.Write(imageData.image, 0, imageData.image.Length);
                    }
                }
            }
            ZipArchiveEntry newEntry = zipArchive.CreateEntry("Config/Findings_Database/Findings_Database.tex");
            using(StreamWriter writer = new StreamWriter(newEntry.Open()))
            {
                writer.Write(projectReportData.FindingsDatabase);
            }
        }

    }
}
