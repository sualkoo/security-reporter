using Newtonsoft.Json.Linq;
using webapi.ProjectSearch.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using webapi.Service;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace webapi.ProjectSearch.Services
{
    public class ProjectReportService : IProjectReportService
    {
        public IProjectDataValidator Validator { get; set; }
        public IProjectDataParser Parser { get; set; }
        public ICosmosService CosmosService { get; set; }
        private readonly ILogger Logger;

        public ProjectReportService(IProjectDataParser parser, IProjectDataValidator validator, ICosmosService cosmosService)
        {
            Validator = validator;
            Parser = parser;
            CosmosService = cosmosService;
            ILoggerFactory loggerFactory = LoggerProvider.GetLoggerFactory();
            Logger = loggerFactory.CreateLogger<ProjectDataValidator>();
        }

        public async Task<ProjectReportData> GetReportByIdAsync(Guid id)
        {
            Logger.LogInformation($"Fetching project report by ID");
            return await CosmosService.GetProjectReport(id.ToString());
        }

        public async Task<List<ProjectReportData>> GetReportsAsync(string? subcategory, string keyword, string value)
        {
            Logger.LogInformation($"Fetching project reports by keywords");

            if (string.IsNullOrEmpty(keyword) || string.IsNullOrEmpty(value))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Missing parameters.");
            }

            if (!string.IsNullOrEmpty(subcategory))
            {
                return await CosmosService.GetProjectReports(subcategory, keyword, value);
            }
            else
            {
                return await CosmosService.GetProjectReports(null, keyword, value);
            }
        }

        public async Task<ProjectReportData> SaveReportFromZip(IFormFile file)
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
                catch (Exception)
                {
                    throw new CustomException(StatusCodes.Status406NotAcceptable, "Zip file has some missing files/missing information in the template.");
                }
            }

            bool isValid = Validator.Validate(newReportData);

            if (isValid == false)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "ProjectReport has missing information.");
            }

            bool result = await CosmosService.AddProjectReport(newReportData);

            if (!result)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "Failed to save ProjectReport to database.");
            }
            return newReportData;
        }
    }
}
