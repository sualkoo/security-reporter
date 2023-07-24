using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("project-reports")]
    public class ProjectReportController : Controller
    {
        private ProjectReportService ProjectReportService { get; }
        private readonly ILogger Logger;

        public ProjectReportController(IProjectReportService projectReportService)
        {
            ProjectReportService = (ProjectReportService)projectReportService;
            ILoggerFactory loggerFactory = LoggerProvider.GetLoggerFactory();
            Logger = loggerFactory.CreateLogger<ProjectDataValidator>();
        }

        [HttpPost]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {
            Logger.LogInformation("Received POST request for adding new Project Report");
            try
            {
                ProjectReportData savedReport = await ProjectReportService.SaveReportFromZip(file);
                return Ok(savedReport);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new ErrorResponse(ex.Message, ex.Details));
            }
        }

        [HttpGet]
        public async Task<IActionResult> getProjectReportsAsync(string? subcategory, string keyword, string value, int page)
        {
            Logger.LogInformation($"Received GET request for fetching reports by keywords, params=(subcategory={subcategory},keyword={keyword}, value={value}))");
            PagedDBResults<List<ProjectReportData>> fetchedReports = await ProjectReportService.GetReportsAsync(subcategory, keyword, value, page);
            return Ok(fetchedReports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getProjectReportById(Guid id)
        {
            Logger.LogInformation("Received GET request for fetching reports by ID=" + id);
            try
            {
                ProjectReportData fetchedReport = await ProjectReportService.GetReportByIdAsync(id);
                return Ok(fetchedReport);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> getProjectReportsAsync(string? ProjectName,string? Details, string? Impact, string? Repeatability, string? References, string? CWE, string keyword, string value, int page)
        {
            Logger.LogInformation($"Received GET request for fetching reports by keywords, params=(ProjectNameFilter={ProjectName}, DetailsFilter={Details}," +
                $" ImpactFilter={Impact},RepeatibilityFilter={Repeatability}, ReferencesFilter={References}, CWEFiler={CWE}, value={value}))");
            PagedDBResults<List<ProjectReportData>> fetchedReports = await ProjectReportService.GetReportsAsync(ProjectName, Details, Impact, Repeatability, References, CWE, value, page);
            return Ok(fetchedReports);
        }
    }
}
