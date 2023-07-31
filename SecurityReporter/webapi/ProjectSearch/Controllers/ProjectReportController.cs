using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Text;

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

        [HttpGet]
        public async Task<IActionResult> getProjectReportsAsync(string? subcategory, string keyword, string value, int page)
        {
            Logger.LogInformation("Received GET request for fetching reports");
            return await HandleExceptionAsync(async () =>
            {
                PagedDBResults<List<ProjectReportData>> fetchedReports = await ProjectReportService.GetReportsAsync(subcategory, keyword, value, page);
                return Ok(fetchedReports);
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
        public async Task<IActionResult> getProjectReportFindings(string? ProjectName, string? Details, string? Impact, string? Repeatability, string? References, string? CWE, string value, int page)
        {
            Logger.LogInformation($"Received GET request for fetching reports by keywords, params=(ProjectNameFilter={ProjectName}, DetailsFilter={Details}," +
                $" ImpactFilter={Impact},RepeatibilityFilter={Repeatability}, ReferencesFilter={References}, CWEFiler={CWE}, value={value}))");
            return await HandleExceptionAsync(async () =>
            {
                PagedDBResults<List<FindingResponse>> fetchedReports = await ProjectReportService.GetReportFindingsAsync(ProjectName, Details, Impact, Repeatability, References, CWE, value, page);
                return Ok(fetchedReports);
            });

        }

        [HttpGet("download")]
        public IActionResult downloadProjectZip()
        {
            try
            {
                byte[] zipData = ProjectReportService.GetProjectZipFile();

                if (zipData == null || zipData.Length == 0)
                {
                    Logger.LogInformation("Empty zip file data.");
                    return NotFound(); // Return a 404 Not Found response if the zip file data is empty.
                }

                // Set appropriate response headers
                var contentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                contentDisposition.FileName = "project.zip";
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                Response.Headers.Add("Content-Length", zipData.Length.ToString());
                Response.ContentType = "application/zip";
                
                // Return the zip file data directly as a FileContentResult
                return new FileContentResult(zipData, "application/zip");
            }
            catch (Exception ex)
            {
                // Log and return an error response if something goes wrong
                Logger.LogError($"Error serving the zip file: {ex}");
                return StatusCode(500, $"Error serving the zip file: {ex.Message}");
            }
        }



    }
}
