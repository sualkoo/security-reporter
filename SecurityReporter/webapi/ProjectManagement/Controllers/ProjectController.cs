using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Service;

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
        Console.WriteLine("Post request Project/add called.");
        bool result = await CosmosService.AddProject(project);


        if (!result)
        {
            Console.WriteLine("Error occured in Project/add post request.");
            return StatusCode(400, "Error: Unable to insert project into DB.");
        }

        Console.WriteLine("Request executed without any errors.");
        return StatusCode(201, project);
    }
}
