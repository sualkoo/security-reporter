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
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("POST");
        Console.ResetColor();
        Console.WriteLine("\t /Project/add");

        bool result = await CosmosService.AddProject(project);

        if (!result)
        {
            Console.WriteLine("Error occured in Project/add post request.");
            return StatusCode(400, "Error: Unable to insert project into DB.");
        }
        Console.WriteLine("Request executed without any errors.");
        return StatusCode(201, project);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteProject(List<string> ids)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("POST");
        Console.ResetColor();
        Console.WriteLine("\t /Project/delete");

        var result = await CosmosService.DeleteProjects(ids);

        if (result.Count > 0)
        {
            return StatusCode(404, result);
        }
        return StatusCode(200, "Ok");
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteProject(string id)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("DELETE");
        Console.ResetColor();
        Console.WriteLine("\t /Project/delete");

        var result = await CosmosService.DeleteProject(id);

        if (!result)
        {
            return StatusCode(404, $"{id}, Not found.");
        }
        return StatusCode(200, "Ok");
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetNumberOfProjects()
    {
        int count = await CosmosService.GetNumberOfProjects();

        if (count < 0)
        {
            return StatusCode(400, count);
        }

        return StatusCode(200, count);
    }

    [HttpGet("retrieve")]
    public async Task<IActionResult> GetItems([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] FilterData filter)
    {
        var items = new List<ProjectData>();

        items = await CosmosService.GetItems(pageSize, pageNumber, filter);

        return Ok(items);
    }

}