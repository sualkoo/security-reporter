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
        private IProjectDataParser _ProjectDataParser { get; set; }
        private ICosmosService _CosmosService { get; set; }

        public ProjectReportController(IProjectDataParser parser, ICosmosService cosmosService)
        {
            _ProjectDataParser = parser;
            _CosmosService = cosmosService;
        }

        [HttpPost]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {
            Console.WriteLine("Received POST request for adding new Project Report");
            ProjectReportData newReportData = _ProjectDataParser.Extract(file.OpenReadStream());
            bool result = await _CosmosService.AddProjectReport(newReportData);
            if (!result)
            {
                return StatusCode(500, "An error occured while saving Project Report to database");
            }
            return StatusCode(201, newReportData);
        }

        [HttpGet]
        public IActionResult getProjectReports()
        {
            Console.WriteLine("Received GET request for fetching Project Reports");
            return StatusCode(501, "We are currently working on this feature.");

        }
    }
}
