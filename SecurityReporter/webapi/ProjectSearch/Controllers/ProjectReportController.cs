using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("project-reports")]
    public class ProjectReportController : ExceptionHandlingControllerBase
    {
        private ProjectReportService ProjectReportService { get; }
        private readonly ILogger Logger;

        public ProjectReportController(ILoggerFactory loggerFactory, IProjectReportService projectReportService) : base(loggerFactory)
        {
            ProjectReportService = (ProjectReportService)projectReportService;
            Logger = loggerFactory.CreateLogger<ProjectReportController>();
        }

        [HttpPost]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {
            Logger.LogInformation("Received POST request for saving new report");
            return await HandleExceptionAsync(async () =>
            {
                ProjectReportData savedReport = await ProjectReportService.SaveReportFromZip(file);
                return Ok(savedReport);
            });

        }

        //[HttpGet]
        //public async Task<IActionResult> getProjectReportsAsync(string? subcategory, string keyword, string value, int page)
        //{
        //    Logger.LogInformation("Received GET request for fetching reports");
        //    return await HandleExceptionAsync(async () =>
        //    {
        //        PagedDBResults<List<ProjectReportData>> fetchedReports = await ProjectReportService.GetReportsAsync(subcategory, keyword, value, page);
        //        return Ok(fetchedReports);
        //    });

        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> getProjectReportById(Guid id)
        {
            Logger.LogInformation("Received GET request for fetching report by ID");
            return await HandleExceptionAsync(async () =>
            {
                ProjectReportData fetchedReport = await ProjectReportService.GetReportByIdAsync(id);
                return Ok(fetchedReport);
            });
        }

        [HttpGet]
        public async Task<IActionResult> getProjectReportsAsyncByParams(string? ProjectName, string? Details, string? Impact, string? Repeatability, string? References, string? CWE, string value, int page)
        {
            Logger.LogInformation($"Received GET request for fetching reports by keywords, params=(ProjectNameFilter={ProjectName}, DetailsFilter={Details}," +
                $" ImpactFilter={Impact},RepeatibilityFilter={Repeatability}, ReferencesFilter={References}, CWEFiler={CWE}, value={value}))");
            return await HandleExceptionAsync(async () =>
            {
                PagedDBResults<List<FindingResponse>> fetchedReports = await ProjectReportService.GetReportsAsync(ProjectName, Details, Impact, Repeatability, References, CWE, value, page);
                return Ok(fetchedReports);
            });

        }
    }
}
