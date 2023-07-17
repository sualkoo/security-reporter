using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;
using webapi.Service;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectReportController : Controller
    {
        private IProjectDataParser _ProjectDataParser { get; set; }
        private ICosmosService _CosmosService { get; set; }

        public ProjectReportController(IProjectDataParser parser, ICosmosService cosmosService)
        {
            _ProjectDataParser = parser;
            _CosmosService = cosmosService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {
            Console.WriteLine("Received POST request for adding ProjectReport");
            ProjectReportData newReportData = _ProjectDataParser.Extract(file.OpenReadStream());
            bool result = await _CosmosService.AddProjectReport(newReportData);
            if (!result)
            {
                return StatusCode(500, "An error occured while saving Project Report to database");
            }
            return StatusCode(201, newReportData);
        }
    }
}
