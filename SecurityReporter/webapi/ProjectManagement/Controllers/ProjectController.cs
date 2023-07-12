using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Service;
using webapi.ProjectManagement.Utils;

namespace webapi.ProjectManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController : ControllerBase
{
    public ICosmosService CosmosService { get; }

    public ProjectController(ICosmosService cosmosService)
    {
        CosmosService = cosmosService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> PostProject(ProjectData project)
    {

        var validator = new ProjectDataValidation();
        if (!validator.IsValid(project))
        {
           // TODO merge errorlist and json
            return StatusCode(400, "");
        }

       bool result = await CosmosService.AddProject(project);

       if (!result)
       {
            return StatusCode(400, "Error: Unable to insert project into DB.");
       }

        return StatusCode(201, project);
    }
}
