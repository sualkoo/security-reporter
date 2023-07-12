using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Service;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController : ControllerBase
{

    [HttpPost("add")]
    public async Task<IActionResult> PostProject(ProjectData project)
    {      
       var db = new DBWrapper();
       bool result = await db.AddProject(project);

       if (!result)
       {
            return StatusCode(400, "Error: Unable to insert project into DB.");
       }

        return StatusCode(201, project);
    }

    [HttpPost("addProjectReport")]
    public async Task<IActionResult> InsertProjectReport(ProjectReportData project)
    {
        var db = new DBWrapper();
        bool result = await db.AddProjectReport(project);

        if (!result)
        {
            return StatusCode(400, "Error: Can't insert Project Report into the database");
        }

        return StatusCode(201, project);
    }
}
