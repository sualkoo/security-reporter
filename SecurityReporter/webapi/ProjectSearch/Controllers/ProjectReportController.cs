using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.Service;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectReportController : Controller
    {
        public ICosmosService CosmosService { get; }

        public ProjectReportController(ICosmosService cosmosService)
        {
            CosmosService = cosmosService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {
            // Process or save the uploaded file as needed

            return Ok("File uploaded successfully.");
        }
    }
}
