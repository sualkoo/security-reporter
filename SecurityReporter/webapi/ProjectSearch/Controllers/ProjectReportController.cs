using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectReportController : Controller { 
        public IProjectDataParser ProjectDataParser { get; set; }
        public ProjectReportController (IProjectDataParser parser ) {
            ProjectDataParser = parser;
        }
    
        [HttpPost("add")]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {
            ProjectReportData newReportData = ProjectDataParser.Extract(file.OpenReadStream());

            // Process or save the uploaded file as needed
            

            return Ok(newReportData);
        }
    }
}
