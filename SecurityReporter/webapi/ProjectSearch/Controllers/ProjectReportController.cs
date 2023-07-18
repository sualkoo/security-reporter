using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
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
        public ProjectDataValidator Validator { get; set; }
        public ProjectDataParser Parser { get; set; }
        public CosmosService CosmosService { get; set; }

        public ProjectReportController(IProjectDataParser parser, IProjectDataValidator validator, ICosmosService cosmosService)
        {
            Validator = (ProjectDataValidator)validator;
            Parser = (ProjectDataParser)parser;
            CosmosService = (CosmosService)cosmosService;
        }

        [HttpPost]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {
            Console.WriteLine("Received POST request for adding new Project Report");
            ProjectReportData newReportData;
            try
            {
                newReportData = Parser.Extract(file.OpenReadStream());
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while extracting data from zip file.");
                return BadRequest("Zip file has some missing files. Make sure you use the most recent version of the LaTeX template");
            };

            bool isValid = Validator.Validate(newReportData);

            if (isValid == false)
            {
                return BadRequest("ProjectReport has missing information");
            }

            bool result = await CosmosService.AddProjectReport(newReportData);

            if (!result)
            {
                return BadRequest("Failed to save ProjectReport to database.");
            }
            Console.WriteLine("New ProjectReport was successfully created and saved to database.");
            return StatusCode(201, newReportData);
        }

        [HttpGet] // Todo: This should return paginated result - For performance reasons
        public async Task<IActionResult> getProjectReports(string? subcategory, string keyword, string value)
        {
            Console.WriteLine($"Fetching project reports by keyword, params=(keyword={keyword}, value={value})");

            if (string.IsNullOrEmpty(keyword) || string.IsNullOrEmpty(value))
            {
                return BadRequest("Missing required parameters.");
            }

            if (!string.IsNullOrEmpty(subcategory)) 
            {
                List<ProjectReportData> projectReports = await CosmosService.GetProjectReports(subcategory, keyword, value);
                return Ok(projectReports);
            } else {
                List<ProjectReportData> projectReports = await CosmosService.GetProjectReports(null, keyword, value);
                return Ok(projectReports);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getProjectReportById(Guid id) {
            Console.WriteLine($"Fetching project report by ID: {id}");
            try
            {
                ProjectReportData data = await CosmosService.GetProjectReport(id.ToString());
                Console.WriteLine("Successfully fetched the Project Report.");
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred while fetching the Project Report.");
                Console.WriteLine(ex);
                return NotFound("Project Report not found.");
            }
        }
    }
}
