using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Services;

namespace webapi.ProjectSearch.Controllers;

[ApiController]
[Route("api/project-reports")]
// [Authorize(Policy = "AdminPentesterPolicy")]
public class ProjectReportController : ExceptionHandlingControllerBase
{
    private new readonly ILogger Logger;

    public ProjectReportController(ILogger<ProjectReportController> logger,
        ILogger<ExceptionHandlingControllerBase> baseLogger, IProjectReportService projectReportService,
        IPdfBuilder pdfBuilder) : base(baseLogger)
    {
        ProjectReportService = projectReportService;
        PdfBuilder = pdfBuilder;
        Logger = logger;
    }

    private IProjectReportService ProjectReportService { get; }
    private IPdfBuilder PdfBuilder { get; }

    [HttpPost]
    public async Task<IActionResult> AddProjectReport(IFormFile file)
    {
        Logger.LogInformation("Received POST request for saving new report");
        return await HandleExceptionAsync(async () =>
        {
            var savedReport = await ProjectReportService.SaveReportFromZipAsync(file);
            return Ok(savedReport);
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectReportById(Guid id)
    {
        Logger.LogInformation("Received GET request for fetching report by ID");
        return await HandleExceptionAsync(async () =>
        {
            var fetchedReport = await ProjectReportService.GetReportByIdAsync(id);
            return Ok(fetchedReport);
        });
    }

    [HttpGet("findings")]
    public async Task<IActionResult> GetProjectReportFindings(string? ProjectName, string? Details, string? Impact,
        string? Repeatability, string? References, string? CWE, string? FindingName, int page)
    {
        Logger.LogInformation(
            $"Received GET request for fetching reports by keywords, params=(ProjectNameFilter={ProjectName}, DetailsFilter={Details}," +
            $" ImpactFilter={Impact},RepeatibilityFilter={Repeatability}, ReferencesFilter={References}, CWEFiler={CWE}, FindingName={FindingName}))");
        return await HandleExceptionAsync(async () =>
        {
            var fetchedReports = await ProjectReportService.GetReportFindingsAsync(ProjectName, Details, Impact,
                Repeatability, References, CWE, FindingName, page);
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
            return await ProjectReportService.GetPdfByProjectIdAsync(id);
        });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProjectReports([FromBody] List<string> ids)
    {
        Logger.LogInformation("Recieved DELETE request for deleting reports by id.");

        return await HandleExceptionAsync(async () =>
        {
            var test = await ProjectReportService.DeleteReportAsync(ids);
            return Ok(test);
        });
    }

    [HttpDelete("all")]
    public async Task<IActionResult> DeleteProjectReportsAll()
    {
        Logger.LogInformation("Received DELETE request for deleting ALL reports.");

        return await HandleExceptionAsync(async () =>
        {
            var result = await ProjectReportService.DeleteReportAllAsync();
            return Ok(result);
        });
    }
}