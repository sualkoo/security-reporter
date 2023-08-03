using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
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
        DBScopeAndProceduresExtractor scopeAndProceduresExtractor;
        DBTestingMethodologyExtractor testingMethodologyExtractor;

        public DBProjectDataParser(ILogger<DBProjectDataParser> logger)
        {
            Logger = logger;
            documentInformationExtractor = new DBDocumentInformationExtractor();
            executiveSummaryExtractor = new DBExecutiveSummaryExtractor();
            projectInformationExtractor = new DBProjectInformationExtractor();
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

                byte[] zipFileBytes = CreateZipFile(fileContents);

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
        private byte[] CreateZipFile(Dictionary<string, byte[]> fileContents)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var entry in fileContents)
                    {
                        var zipEntry = zipArchive.CreateEntry(entry.Key);
                        using (var entryStream = zipEntry.Open())
                        {
                            entryStream.Write(entry.Value, 0, entry.Value.Length);
                        }
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}
