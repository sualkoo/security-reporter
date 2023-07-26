using Microsoft.AspNetCore.Mvc;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectReportController : Controller
    {
        [HttpPost("add")]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {
            return Ok("File uploaded successfully.");
        }
    }
}
