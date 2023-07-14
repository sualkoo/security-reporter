using Microsoft.AspNetCore.Mvc;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;

namespace webapi.ProjectSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectReportController : Controller
    {
        public ProjectDataValidator Validator { get; set; }
        public ProjectReportController(IProjectDataValidator validator) {
            Validator = (ProjectDataValidator)validator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> addProjectReport(IFormFile file)
        {

            ProjectReportData data = new ProjectReportData();
            data.DocumentInfo = new webapi.Models.ProjectReport.DocumentInformation();
            data.DocumentInfo.ProjectReportName = "pokus";
            webapi.Models.ProjectReport.Finding item = new webapi.Models.ProjectReport.Finding();
            data.Findings.Add(item);
            data.Findings[1].FindingAuthor = "";
            bool dataHasValidationErrors = Validator.Validate(data);

            if (!dataHasValidationErrors)
            {
                //v buducnosti list Errorov.
                return BadRequest("ProjectReport is missing required fields!");
            }
            return Ok("File uploaded successfully.");
        }
    }
}
