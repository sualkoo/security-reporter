using webapi.ProjectSearch.Models;
using webapi.Service;


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

        public async Task<PagedDBResults<List<FindingResponse>>> GetReportFindingsAsync(string? projectName, string? details, string? impact, string? repeatability, string? references, string? cWE, string value, int page)
        {
            Logger.LogInformation($"Fetching project reports by keywords");

            if (string.IsNullOrEmpty(value))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Missing parameters.");
            }

            return await CosmosService.GetPagedProjectReportFindings(projectName, details, impact, repeatability, references, cWE, value, page);
        }

        public async Task<bool> DeleteReportAsync(List<string> ids)
        {
          
            return true;
        }
    }
}
