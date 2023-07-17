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

        [HttpGet]
        public IActionResult getProjectReports(string? id, string? projectReportName)
        {
            Console.WriteLine("Id: " + id);
            Console.WriteLine($"Received GET request for fetching Project Reports, params=(id={id},projectReportname={projectReportName})");
            if (!string.IsNullOrEmpty(id))
            {
                // Fetch project report by ID (should return only 1 item)
                Console.WriteLine("Fetching by id");
                return StatusCode(501, "We are currently working on this feature.");
            }

            if (!string.IsNullOrEmpty(projectReportName))
            {
                // Fetch project reports by 'projectReportName'
                Console.WriteLine("Fetching by project report name");
                return StatusCode(501, "We are currently working on this feature.");
            }

            return StatusCode(501, "We are currently working on this feature.");
        }
    }
}
