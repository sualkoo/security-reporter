using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Newtonsoft.Json.Linq;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;
using webapi.Service;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("project-reports")]
    public class ProjectReportController : Controller
    {
        private ProjectReportService ProjectReportService { get; }

        public ProjectReportController(IProjectReportService projectReportService)
        {
            ProjectReportService = (ProjectReportService)projectReportService;
        }

        [HttpPost]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {
            Console.WriteLine("Received POST request for adding new Project Report");
            try
            {
                ProjectReportData savedReport = await ProjectReportService.SaveReportFromZip(file);
                return Ok(savedReport);
            } catch (CustomException ex) {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> getProjectReportsAsync(string? subcategory, string keyword, string value)
        {
            
            Console.WriteLine($"Received GET request for fetching reports by keywords, params=(subcategory={subcategory},keyword={keyword}, value={value}))");
            List <ProjectReportData> fetchedReports = await ProjectReportService.GetReportsAsync(subcategory, keyword, value);
            // Todo: This should return paginated result - For performance reasons
            return Ok(fetchedReports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getProjectReportById(Guid id)
        {
            Console.WriteLine("Received GET request for fetching reports by ID=" + id);
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
    }
}
