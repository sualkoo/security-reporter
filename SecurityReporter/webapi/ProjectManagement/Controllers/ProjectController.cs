using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Service;

namespace webapi.ProjectManagement.Controllers;

[Authorize]
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
    [Authorize(Policy = "AdminCoordinatorPolicy")]
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
    [Authorize(Policy = "AdminCoordinatorPolicy")]
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
    [Authorize(Policy = "AdminCoordinatorPolicy")]
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
    [Authorize(Policy = "AdminCoordinatorClientPolicy")]
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
    [Authorize(Policy = "AdminCoordinatorClientPolicy")]
    public async Task<IActionResult> GetItems([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] FilterData filter)
    {
        var items = new List<ProjectData>();

        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("GET");
            Console.ResetColor();
            Console.WriteLine("\t /Project/retrieve");

            items = await CosmosService.GetItems(pageSize, pageNumber, filter);

            if (items.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success: Data retrieved successfully.");
                Console.ResetColor();

                return StatusCode(200, items);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Info: No data found matching the filter criteria.");
                Console.ResetColor();

                return StatusCode(404, "No data found matching the filter criteria.");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();

            return StatusCode(400, $"Error retrieving data: {ex.Message}");
        }
    }

    [HttpPut("update")]
    [Authorize(Policy = "AdminPAdminCoordinatorPolicy")]
    public async Task<IActionResult> UpdateProject(ProjectData project)
    {
        Console.WriteLine("Updating project..");
        
        bool result = await CosmosService.UpdateProject(project);
        
        if (!result)
        {
            Console.WriteLine("Error occured in Project/update put request.");
            return StatusCode(400, "Error: Unable to update project in DB.");
        }

        Console.WriteLine("Request executed without any errors.");
        return StatusCode(202, project);
    }

    [HttpGet("getProject")]
    [Authorize(Policy = "AdminCoordinatorPolicy")]
    public async Task<IActionResult> GetProjectById(string id)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("GET");
        Console.ResetColor();
        Console.WriteLine($"\t /Project/{id}");

        try
        {
            var project = await CosmosService.GetProjectById(id);

            if (project != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success: Project data retrieved successfully.");
                Console.ResetColor();

                return StatusCode(200, project);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Info: Project with ID '{id}' not found.");
                Console.ResetColor();

                return StatusCode(404, $"Project with ID '{id}' not found.");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();

            return StatusCode(400, $"Error retrieving project data: {ex.Message}");
        }
    }
}
    