using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;
using webapi.Service;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("ProjectReports")]
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
            ProjectReportData newReportData = null;
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

        // Todo: Implement query parameters like we did with CosmosService getProjectReports so we can flexibly filter data at frontend
        [HttpGet]
        public async Task<IActionResult> getProjectReports(string? id, string? projectReportName)
        {
            Console.WriteLine($"Received GET request for fetching Project Reports, params=(id={id},projectReportname={projectReportName})");
            if (!string.IsNullOrEmpty(id))
            {
                // Fetch project report by ID (should return only 1 item)
                Console.WriteLine($"Fetching project report with ID: {id}");

                try
                {
                    ProjectReportData data = await CosmosService.GetProjectReport(id);
                    Console.WriteLine("Successfully fetched the Project Report.");
                    return Ok(data);
                }
                catch (Exception)
                {
                    Console.WriteLine("An exception occurred while fetching the Project Report.");
                    return NotFound("Project Report not found.");
                }
            }

            if (!string.IsNullOrEmpty(projectReportName))
            {
                // Fetch project reports by 'projectReportName'
                Console.WriteLine($"Fetching project reports by name: {projectReportName}");
                List<ProjectReportData> projectReports = await CosmosService.GetProjectReports("DocumentInfo", "ProjectReportName", projectReportName);
                // Todo: This should return paginated result - For performance reasons
                return Ok(projectReports);
            }

            Console.WriteLine("Invalid request. Either ID or ProjectReportName must be specified.");
            return BadRequest("Either ID or ProjectReportName must be specified.");
        }
    }
}
