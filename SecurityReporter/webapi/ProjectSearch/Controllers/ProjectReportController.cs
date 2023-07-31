using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("project-reports")]
    public class ProjectReportController : ExceptionHandlingControllerBase
    {
        private IProjectReportService ProjectReportService { get; }
        private readonly ILogger Logger;

        public ProjectReportController(ILogger<ProjectReportController> logger, IProjectReportService projectReportService)
        {
            ProjectReportService = projectReportService;
            Logger = logger;
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

        [HttpGet("findings")]
        public async Task<IActionResult> getProjectReportFindings(string? ProjectName, string? Details, string? Impact, string? Repeatability, string? References, string? CWE, int page)
        {
            Logger.LogInformation($"Received GET request for fetching reports by keywords, params=(ProjectNameFilter={ProjectName}, DetailsFilter={Details}," +
                $" ImpactFilter={Impact},RepeatibilityFilter={Repeatability}, ReferencesFilter={References}, CWEFiler={CWE}))");
            return await HandleExceptionAsync(async () =>
            {
                PagedDBResults<List<FindingResponse>> fetchedReports = await ProjectReportService.GetReportFindingsAsync(ProjectName, Details, Impact, Repeatability, References, CWE, page);
                return Ok(fetchedReports);
            });

        }
    }
}
