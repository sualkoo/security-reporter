using Microsoft.AspNetCore.Mvc;
using webapi.Service;


namespace webapi.MyProfile.Controllers;



[ApiController]
public class MyProfileController : ControllerBase
{
    public ICosmosService CosmosService { get; }

    public MyProfileController(ICosmosService cosmosService)
    {
        CosmosService = cosmosService;
    }



    [HttpGet("profile")]
    public async Task<IActionResult> GetByEmail([FromQuery] int pageSize, [FromQuery] int pageNumber)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("GET");
        Console.ResetColor();
        Console.WriteLine("\t /profile");

        var result = await CosmosService.GetBacklog(pageSize, pageNumber);

        if (result.Projects.Count() == 0)
        {
            Console.WriteLine("No items found for given name.");
            return StatusCode(404, "Error: No items found for given name.");
        }

        Console.WriteLine("Request executed without any errors.");
        return StatusCode(201, result);
    }

}
