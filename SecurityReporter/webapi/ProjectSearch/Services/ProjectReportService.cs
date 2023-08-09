using System.Text;
using webapi.ProjectSearch.Models;
using webapi.Service;
using Ionic.Zip;
using System.IO;
using System.Web;
using webapi.ProjectSearch.Services.Extractor;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;

namespace webapi.ProjectSearch.Services
{
    public class ProjectReportService : IProjectReportService
    {
        private IProjectDataValidator Validator { get; }
        private IProjectDataParser Parser { get; set; }
        private IDBProjectDataParser DBParser { get; }
        private ICosmosService CosmosService { get; }
        private IPDFBuilder PdfBuilder { get; }
        private readonly ILogger Logger;

        public ProjectReportService(IProjectDataParser parser, IDBProjectDataParser dbParser,
            IProjectDataValidator validator, ICosmosService cosmosService, IPDFBuilder pdfBuilder)
        {
            Validator = validator;
            Parser = parser;
            DBParser = dbParser;
            CosmosService = cosmosService;
            PdfBuilder = pdfBuilder;
            ILoggerFactory loggerFactory = LoggerProvider.GetLoggerFactory();
            Logger = loggerFactory.CreateLogger<ProjectDataValidator>();
        }

        async Task<ProjectReportData> IProjectReportService.GetReportByIdAsync(Guid id)
        {
            Logger.LogInformation($"Fetching project report by ID");
            return await CosmosService.GetProjectReport(id.ToString());
        }

        async Task<ProjectReportData> IProjectReportService.SaveReportFromZip(IFormFile file)
        {
            Logger.LogInformation($"Saving new project report");
            ProjectReportData newReportData;

            if (Path.GetExtension(file.FileName)?.ToLower() != ".zip")
            {
                throw new CustomException(StatusCodes.Status406NotAcceptable,
                    "Invalid file type. Only .zip files are allowed.");
            }
            else
            {
                try
                {
                    newReportData = Parser.Extract(file.OpenReadStream());
                }
                catch (Exception ex)
                {
                    throw new CustomException(StatusCodes.Status406NotAcceptable, ex.Message);
                }
            }

            Validator.Validate(newReportData);

            bool result = await CosmosService.AddProjectReport(newReportData);

            if (!result)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError,
                    "Failed to save ProjectReport to database.");
            }

            bool pdfGenerated = false;
            try
            {
                pdfGenerated = await createPDF(file.OpenReadStream(),
                    "Report-" + newReportData?.DocumentInfo?.ProjectReportName?.Replace(" ", "_") ?? "Report-Untitled",
                    newReportData.Id.ToString());
            }
            finally
            {
                if (!pdfGenerated)
                {
                    Logger.LogInformation("PDF generation failed, deleting project report...");
                    await CosmosService.DeleteProjectReports(new List<string> {newReportData.Id.ToString()});
                }
            }

            return newReportData;
        }

        async Task<PagedDBResults<List<FindingResponse>>> IProjectReportService.GetReportFindingsAsync(
            string? projectName, string? details, string? impact, string? repeatability, string? references,
            string? cWE, int page)
        {
            Logger.LogInformation($"Fetching project reports by keywords");
            if (!string.IsNullOrEmpty(projectName) && !string.IsNullOrEmpty(details) &&
                !string.IsNullOrEmpty(impact) && !string.IsNullOrEmpty(repeatability) &&
                !string.IsNullOrEmpty(references) &&
                !string.IsNullOrEmpty(cWE))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Missing parameters.");
            }

            return await CosmosService.GetPagedProjectReportFindings(projectName, details, impact, repeatability,
                references, cWE, page);
        }

        async Task<bool> IProjectReportService.DeleteReportAsync(List<string> ids)
        {
            Logger.LogInformation($"Deleting items from database");

            return await CosmosService.DeleteProjectReports(ids);
        }

        public async Task<FileContentResult> GetReportSourceByIdAsync(Guid id)
        {
            ProjectReportData data = await CosmosService.GetProjectReport(id.ToString());
            FileContentResult zip = DBParser.Extract(data);
            return zip;
        }

        // Todo: Store the PDF in blob storage
        public async Task<bool> createPDF(Stream zipFileStream, string outputPDFname, string projectReportId)
        {
            FileContentResult generatedPdf = await PdfBuilder.GeneratePDFFromZip(zipFileStream, outputPDFname);

            // Store the PDF inside working directory
            string workingDirectory = Path.Combine(Environment.CurrentDirectory, "temp", "pdf");
            string filePath = Path.Combine(workingDirectory, $"{projectReportId}.pdf");

            Directory.CreateDirectory(workingDirectory);
            File.WriteAllBytes(filePath, generatedPdf.FileContents);

            Console.WriteLine($"Content saved to {filePath}");
            return true;
        }

        public async Task<FileContentResult> GetPDFByProjectId(Guid id)
        {
            string workingDirectory = Path.Combine(Environment.CurrentDirectory, "temp", "pdf");
            string fileNameToSearch = $"{id}.pdf"; // Assuming the ID is used in the filename

            string filePath = Path.Combine(workingDirectory, fileNameToSearch);

            if (!File.Exists(filePath))
            {
                throw new CustomException(StatusCodes.Status404NotFound, "PDF was not found for this project.");
            }

            byte[] fileContent = await File.ReadAllBytesAsync(filePath);
            return new FileContentResult(fileContent, "application/pdf")
            {
                FileDownloadName = id.ToString()
            };
        }
    }
}