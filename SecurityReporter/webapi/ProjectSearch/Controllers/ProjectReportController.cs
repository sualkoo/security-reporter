using System.Text;
using System.Text.Unicode;
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
        private IPdfBuilder PDFBuilder { get; }
        private readonly ILogger Logger;

        public ProjectReportController(ILogger<ProjectReportController> logger, ILogger<ExceptionHandlingControllerBase> baseLogger, IProjectReportService projectReportService, IPdfBuilder pdfBuilder) :base(baseLogger)
        {
            ProjectReportService = projectReportService;
            PDFBuilder = pdfBuilder;
            Logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectReport(IFormFile file)
        {
            Logger.LogInformation("Received POST request for saving new report");
            return await HandleExceptionAsync(async () =>
            {
                ProjectReportData savedReport = await ProjectReportService.SaveReportFromZip(file);
                return Ok(savedReport);
            });

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectReportById(Guid id)
        {
            Logger.LogInformation("Received GET request for fetching report by ID");
            return await HandleExceptionAsync(async () =>
            {
                ProjectReportData fetchedReport = await ProjectReportService.GetReportByIdAsync(id);
                return Ok(fetchedReport);
            });
        }

        [HttpGet("findings")]
        public async Task<IActionResult> GetProjectReportFindings(string? ProjectName, string? Details, string? Impact, string? Repeatability, string? References, string? CWE, int page)
        {
            Logger.LogInformation($"Received GET request for fetching reports by keywords, params=(ProjectNameFilter={ProjectName}, DetailsFilter={Details}," +
                $" ImpactFilter={Impact},RepeatibilityFilter={Repeatability}, ReferencesFilter={References}, CWEFiler={CWE}))");
            return await HandleExceptionAsync(async () =>
            {
                PagedDBResults<List<FindingResponse>> fetchedReports = await ProjectReportService.GetReportFindingsAsync(ProjectName, Details, Impact, Repeatability, References, CWE, page);
                return Ok(fetchedReports);
            });

        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadProjectZip(Guid id)
        {
            Logger.LogInformation("Received GET request for download report by ID");
            return await HandleExceptionAsync(async () =>
            {
                return await ProjectReportService.GetReportSourceByIdAsync(id);
            });
        }

        [HttpGet("{id}/download/pdf")]
        public async Task<IActionResult> DownloadProjectPdf(Guid id)
        {
            Logger.LogInformation("Received GET request for downloading PDF of report by ID");
            return await HandleExceptionAsync(async () =>
            {
                return await ProjectReportService.GetPdfByProjectId(id);
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProjectReports([FromBody] List<string> ids)
        {
            Logger.LogInformation("Recieved DELETE request for deleting reports by id.");

            return await HandleExceptionAsync(async () =>
            {
                bool test = await ProjectReportService.DeleteReportAsync(ids);
                return Ok(test);
            });
        }
    }
}
