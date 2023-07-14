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
            this._ProjectDataParser = parser;
            this._CosmosService = cosmosService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {
            ProjectReportData newReportData = _ProjectDataParser.Extract(file.OpenReadStream());
            await _CosmosService.AddProjectReport(newReportData);

            return Ok(newReportData);
        }
    }
}
