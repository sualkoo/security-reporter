using Microsoft.AspNetCore.Mvc;
using System.Text;
using webapi.ProjectSearch.Models;
using webapi.Service;
using Ionic.Zip;
using System.IO;
using System.Web;
using webapi.ProjectSearch.Services.Extractor;
using webapi.ProjectSearch.Services.Extractor.DBToZipExtract;
using System.IO.Compression;

namespace webapi.ProjectSearch.Services
{
    public class ProjectReportService : IProjectReportService
    {
        public IProjectDataValidator Validator { get; set; }
        public IProjectDataParser Parser { get; set; }
        public IDBProjectDataParser DBParser { get; set; }

        public ICosmosService CosmosService { get; set; }
        private readonly ILogger Logger;

        public ProjectReportService(IProjectDataParser parser,IDBProjectDataParser dbParser, IProjectDataValidator validator, ICosmosService cosmosService)
        {
            Validator = validator;
            Parser = parser;
            DBParser = dbParser;
            CosmosService = cosmosService;
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
                throw new CustomException(StatusCodes.Status406NotAcceptable, "Invalid file type. Only .zip files are allowed.");
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
                    throw new CustomException(StatusCodes.Status406NotAcceptable, "Zip file has some missing files/missing information in the template.");
                }
            }

            Validator.Validate(newReportData);

            bool result = await CosmosService.AddProjectReport(newReportData);

            if (!result)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "Failed to save ProjectReport to database.");
            }
            return newReportData;
        }

        async Task<PagedDBResults<List<FindingResponse>>> IProjectReportService.GetReportFindingsAsync(string? projectName, string? details, string? impact, string? repeatability, string? references, string? cWE, int page)
        {
            Logger.LogInformation($"Fetching project reports by keywords");
            if (!string.IsNullOrEmpty(projectName) && !string.IsNullOrEmpty(details) && 
                !string.IsNullOrEmpty(impact) && !string.IsNullOrEmpty(repeatability) && !string.IsNullOrEmpty(references) && 
                !string.IsNullOrEmpty(cWE))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Missing parameters.");
            }

            return await CosmosService.GetPagedProjectReportFindings(projectName, details, impact, repeatability, references, cWE, page);
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
    }
}
